import React, { useState } from 'react';
import './PreviewPopup.css';

const PreviewPopup = ({ document, onClose, isPreview, onDelete, onUpdate }) => {
    const [isInEditMode, setIsInEditMode] = useState(false);
    const [newName, setNewName] = useState(document.name);
    const typeIconDir = window.initialState.typeIconDir || '';
    const previewStorageDir = window.initialState.previewStorageDir || '';
    
    

    const handleEdit = () => setIsInEditMode(true);
    const handleSave = () => {
        onUpdate({
            id: document.id,
            name: newName,
            downloadsCount: document.downloadsCount,
        });
        setIsInEditMode(false);
    };

    return (
        <div className="preview-popup">
            <div className="preview-header">
                <img src={`${typeIconDir}/${document.fileType.iconFileName}`} alt="file type" />
                <button onClick={onClose}>X</button>
            </div>
            <div className="preview-body">
                {isInEditMode ? (
                    <input type="text" value={newName} onChange={(e) => setNewName(e.target.value)} />
                ) : (
                    <h2>{document.name}</h2>
                )}
                <p>Created Date: {new Date(document.createdDate).toLocaleDateString()}</p>
                <p>Downloads: {document.downloadsCount}</p>
                <img src={`${previewStorageDir}/${document.id}.png`} alt="preview" />
                {!isPreview && (
                    <div className="preview-actions">
                        {isInEditMode ? (
                            <button onClick={handleSave}>Save</button>
                        ) : (
                            <button onClick={handleEdit}>Edit</button>
                        )}
                        <button onClick={() => onDelete(document.id)}>Delete</button>
                    </div>
                )}
            </div>
        </div>
    );
};

export default PreviewPopup;
