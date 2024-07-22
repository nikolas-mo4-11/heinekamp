import React, { useState } from 'react';
import './App.css';
import Header from "./components/Header/Header";
import DocumentTable from "./components/DocumentTable/DocumentTable";
import UploadPopup from "./components/UploadPopup/UploadPopup";
import PreviewPopup from "./components/PreviewPopup/PreviewPopup";
import {postDeleteDocApi, postUpdateDocApi} from "./components/DocumentApi";
import Errors from "./components/Errors/Errors";

const App = () => {
    const [isUploadPopupOpen, setIsUploadPopupOpen] = useState(false);
    const [isPreviewPopupOpen, setIsPreviewPopupOpen] = useState(false);
    const [previewDocument, setPreviewDocument] = useState(null);
    const [errors, setErrors] = useState([]);

    const onErrors = (errorsArr) => {
        setErrors(errorsArr);
    }

    const handleUploadClick = () => setIsUploadPopupOpen(true);
    const handleUploadClose = () => setIsUploadPopupOpen(false);

    const handlePreviewClick = (document) => {
        setPreviewDocument(document);
        setIsPreviewPopupOpen(true);
    };
    const handlePreviewClose = () => setIsPreviewPopupOpen(false);

    const handleUpload = () => {
        setIsUploadPopupOpen(false);
    };

    const handleUpdate = (updateData) => {
        postUpdateDocApi(updateData)
            .then(response => {
                if (!response.ok) {
                    onErrors(['Network response was not ok']);
                }
            })
            .catch(error => onErrors(error));
    };

    const handleDelete = (id) => {
        postDeleteDocApi(id)
            .then(response => {
            if (!response.ok) {
                onErrors(['Network response was not ok']);
            }
        })
            .catch(error => onErrors(error));
        setIsPreviewPopupOpen(false);
    };
    
    const onDocumentDownload = (doc) => {
        handleUpdate({
            id: doc.id,
            name: doc.name,
            downloadsCount: doc.downloadsCount + 1
        })
    }

    return (
        <div className="app">
            <Header 
                onUploadClick={handleUploadClick} 
            />
            {errors.length > 0 &&
                <Errors
                    errors = {errors}
                />}
            <DocumentTable 
                onPreviewClick={handlePreviewClick}
                onErrors={onErrors}
                onDownload={onDocumentDownload}
            />
            {isUploadPopupOpen && 
                <UploadPopup 
                    onClose={handleUploadClose} 
                    onUpload={handleUpload} 
                    onPreviewClick={handlePreviewClick}
                    onErrors={onErrors}
                />}
            {isPreviewPopupOpen &&
                <PreviewPopup 
                    document={previewDocument} 
                    onClose={handlePreviewClose} 
                    isPreview={false} 
                    onUpdate={handleUpdate} 
                    onDelete={handleDelete} 
                />}
        </div>
    );
};

export default App;
