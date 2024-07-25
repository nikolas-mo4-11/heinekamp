import React, {useEffect, useState} from 'react';
import './App.css';
import Header from "./components/Header/Header";
import DocumentTable from "./components/DocumentTable/DocumentTable";
import UploadPopup from "./components/UploadPopup/UploadPopup";
import PreviewPopup from "./components/PreviewPopup/PreviewPopup";
import {getDocumentListApi, postDeleteDocApi, postUpdateDocApi} from "./components/DocumentApi";
import Errors from "./components/Errors/Errors";
import LinkPopup from "./components/LinkPopup/LinkPopup";

const App = () => {
    const documentStorageDir = window.initialState.documentStorageDir || '';

    const [documents, setDocuments] = useState([]);
    const [isUploadPopupOpen, setIsUploadPopupOpen] = useState(false);
    const [errors, setErrors] = useState([]);
    const [currentPreviewDoc, setCurrentPreviewDoc] = useState(null);
    const [isPreviewPopupOpen, setIsPreviewPopupOpen] = useState(false);
    const [isFetching, setIsFetching] = useState(false);
    const [isCreateLinkPopupOpen, setIsCreateLinkPopupOpen] = useState(false);
    const [currentLinkCreationDoc, setCurrentLinkCreationDoc] = useState(null);

    const onErrors = (errorsArr) => {
        setErrors(errorsArr);
    };

    useEffect( () => {
        fetchDocuments();
    }, []);

    const fetchDocuments = () => {
        setIsFetching(true);
        getDocumentListApi()
            .then(response => response.json())
            .then(response => {
                setDocuments(response);
            })
            .then(_ => setIsFetching(false))
            .catch(error => onErrors([error]));
    };
    
    const reloadDocsList = () => {
        fetchDocuments();
    };
    
    const showPreviewPopup = (docToPreview) => {
        setIsPreviewPopupOpen(true);
        setCurrentPreviewDoc(docToPreview);
    };
    
    const closePreviewPopup = () => {
        setCurrentPreviewDoc(null);
        setIsPreviewPopupOpen(false);
        reloadDocsList();
    };

    const showUploadPopup = () => setIsUploadPopupOpen(true);
    const closeUploadPopup = () => setIsUploadPopupOpen(false);

    const showCreateLinkPopup = (doc) => {
        setIsCreateLinkPopupOpen(true);
        setCurrentLinkCreationDoc(doc);
    } ;
    const closeCreateLinkPopup = () => {
        setIsCreateLinkPopupOpen(false);
        setCurrentLinkCreationDoc(null);
    };
        
    
    const onUploadStart = () => {
        setIsFetching(true);
    };

    const onUploadFinish = () => {
        setIsFetching(false);
        reloadDocsList();
    };

    const updateDoc = (doc) => {
        postUpdateDocApi(doc)
            .then(response => {
                if (!response.ok) {
                    onErrors(['Network response was not ok']);
                }
                reloadDocsList();
            })
            .catch(error => onErrors([error]));
    };

    const deleteDoc = (doc) => {
        postDeleteDocApi(doc.id)
            .then(response => {
            if (!response.ok) {
                onErrors(['Network response was not ok']);
            }
            reloadDocsList();
        })
            .catch(error => onErrors([error]));
        closePreviewPopup(false);
    };

    const downloadDocument = (doc) => {
        try{
            const ext = doc.fileType.extension;
            const link = document.createElement('a');
            link.href = `${documentStorageDir}/${doc.id}${ext}`;
            link.download = `${doc.name}${ext}`;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        }
        catch (error){
            onErrors([error.message])
        }
        
        updateDoc({
            id: doc.id,
            name: doc.name,
            downloadsCount: doc.downloadsCount + 1
        });
    };

    return (
        <div className="app">
            {isFetching && <div style={{
                width: 10,
                position: 'fixed',
                top: '50%',
                left: '50%',
                transform: 'translate(-50%, -50%)',
                zIndex: 9999,
                display: 'flex',
                justifyContent: 'center',
                alignItems: 'center',}}>
                <img src={"./assets/loader.gif"} alt="loader"/>
            </div>}

            <div className='wrapper' style={isFetching ? {pointerEvents: "none", opacity: "0.4"} : {}}>
                <Header
                    onUpload={showUploadPopup}
                />
                {errors.length > 0 &&
                    <Errors
                        errors={errors}
                    />}

                <DocumentTable
                    documents={documents}
                    onPreview={showPreviewPopup}
                    onDownload={downloadDocument}
                    onErrors={onErrors}
                    onCreateLink={showCreateLinkPopup}
                />

                {isPreviewPopupOpen &&
                    <PreviewPopup
                        document={currentPreviewDoc}
                        onClose={closePreviewPopup}
                        onDeleteDoc={deleteDoc}
                        onUpdateDoc={updateDoc}
                    />}

                {isUploadPopupOpen &&
                    <UploadPopup
                        onClose={closeUploadPopup}
                        onPreviewClick={showPreviewPopup}
                        onErrors={onErrors}
                        onUploadStart={onUploadStart}
                        onUploadFinish={onUploadFinish}
                    />}

                {isCreateLinkPopupOpen &&
                    <LinkPopup
                        document={currentLinkCreationDoc}
                        onErrors={onErrors}
                        onClose={closeCreateLinkPopup}
                    />}
            </div>

        </div>
    );
};

export default App;
