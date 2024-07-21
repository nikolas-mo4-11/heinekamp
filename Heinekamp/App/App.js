import React, { useState } from 'react';
import './App.css';
import Header from "./components/Header/Header";
import DocumentTable from "./components/DocumentTable/DocumentTable";
import UploadPopup from "./components/UploadPopup/UploadPopup";
import PreviewPopup from "./components/PreviewPopup/PreviewPopup";

const App = () => {
    const [isUploadPopupOpen, setIsUploadPopupOpen] = useState(false);
    const [isPreviewPopupOpen, setIsPreviewPopupOpen] = useState(false);
    const [previewDocument, setPreviewDocument] = useState(null);

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
        postUpdateDocApi(updateData);
    };

    const handleDelete = (id) => {
        postDeleteDocApi(id)
        setIsPreviewPopupOpen(false);
    };

    return (
        <div className="app">
            <Header 
                onUploadClick={handleUploadClick} 
            />
            <DocumentTable 
                onPreviewClick={handlePreviewClick} 
            />
            {isUploadPopupOpen && 
                <UploadPopup 
                    onClose={handleUploadClose} 
                    onUpload={handleUpload} 
                    onPreviewClick={handlePreviewClick} 
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
