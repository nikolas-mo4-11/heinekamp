import React, {useEffect, useState} from 'react';
import './App.css';
import Header from "./components/Header/Header";
import DocumentTable from "./components/DocumentTable/DocumentTable";
import UploadPopup from "./components/UploadPopup/UploadPopup";
import PreviewPopup from "./components/PreviewPopup/PreviewPopup";
import {
    getDocumentListApi,
    postDeleteDocApi,
    postDownloadDocApi,
    postDownloadManyDocsApi,
    postUpdateDocApi
} from "./components/DocumentApi";
import Errors from "./components/Errors/Errors";
import LinkPopup from "./components/LinkPopup/LinkPopup";

const App = () => {
    const documentStorageDir = window.initialState.documentStorageDir || '';

    const [documents, setDocuments] = useState([]);
    const [selectedRowKeys, setSelectedRowKeys] = useState([]);
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
        postDownloadDocApi(doc.id)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.blob();
            })
            .then(blob => handleDownloadFile(blob, doc.name, doc.fileType.extension))
            .then(_ => reloadDocsList())
            .catch(error => onErrors([error]));
    };
    
    const downloadMany = () => {
        if (selectedRowKeys.length === 0){
            onErrors(['No files selected to download']);
            return;
        }

        postDownloadManyDocsApi(selectedRowKeys)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.blob();
            })
            .then(blob => handleDownloadFile(blob, 'files', '.zip'))
            .then(_ => setSelectedRowKeys([]))
            .then(_ => reloadDocsList())
            .catch(error => onErrors([error]));
    }
    
    const handleDownloadFile = (blob, name, extension) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = `${name}${extension}`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
    }

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
                    selectedRowKeys={selectedRowKeys}
                    onDownloadMany={downloadMany}
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
                    selectedRowKeys={selectedRowKeys}
                    setSelectedRowKeys={setSelectedRowKeys}
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
