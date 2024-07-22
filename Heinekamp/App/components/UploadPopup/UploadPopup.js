import React, {useEffect, useState} from 'react';
import './UploadPopup.css';
import {getFileTypesApi, postUploadDocsApi} from "../DocumentApi";

const UploadPopup = ({ onClose, onUpload, onPreviewClick, onErrors }) => {
    const [files, setFiles] = useState([]);
    const [fileTypes, setFileTypes] = useState([]);
    const typeIconDir = window.initialState.typeIconDir || '';

    useEffect(() => {
        fetchFileTypes();
    }, []);

    const fetchFileTypes = () => {
        getFileTypesApi()
            .then(response => response.json())
            .then(response => {
                setFileTypes(response);
            })
            .catch(error => onErrors([error]));
    };

    const handleFileSelect = (event) => {
        setFiles([...files, ...event.target.files]);
    };

    const handleUpload = async () => {
        if (files.length === 0){
            onUpload();
            return;
        }
        
        const formData = new FormData();
        files.forEach(file => formData.append('files', file));
        
        await postUploadDocsApi(formData)
            .then(response => {
                if (!response.ok) {
                    onErrors(['Network response was not ok']);
                }
                
                onUpload();
            })
            .catch(error => onErrors(error));
    };
    
    const getIconFileName = (extension) => {
        const matchedType = fileTypes.find(ft => ft.extension === extension);
        if (matchedType == null){
            throw new Error('Can\'t handle file like this')
        }
        return `${typeIconDir}/${matchedType.iconFileName}`;
    }

    const getFileExtension = (filename) => {
        return `.${filename.split('.').pop()}`; 
    };

    return (
        <div className="upload-popup">
            <div className="upload-header">
                <span>Upload Files</span>
                <button onClick={onClose}>X</button>
            </div>
            <div className="upload-body">
                <table>
                    <thead>
                    <tr>
                        <th>Type</th>
                        <th>Name</th>
                        <th>Preview</th>
                    </tr>
                    </thead>
                    <tbody>
                    {files.map((file, index) => (
                        <tr key={index}>
                            <td>
                                <img className='icon' src={getIconFileName(getFileExtension(file.name))}
                                     alt="file type"/>
                            </td>
                            <td>
                                <input type="text" defaultValue={file.name.replace(/\.[^/.]+$/, "")}/>
                            </td>
                            <td>
                                <button>Preview</button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
                <input type="file" multiple accept=".doc, .docx, .pdf, image/png, image/jpeg, image/jpg, .xls, .xlsx, .txt, .gif" onChange={handleFileSelect}/>
            </div>
            <div className="upload-footer">
                <button onClick={handleUpload}>Upload</button>
                <button onClick={onClose}>Cancel</button>
            </div>
        </div>
    );
};

export default UploadPopup;
