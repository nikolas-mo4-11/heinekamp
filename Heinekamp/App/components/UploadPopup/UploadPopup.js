import React, {useEffect, useState} from 'react';
import './UploadPopup.css';

const UploadPopup = ({ onClose, onUpload, onPreviewClick }) => {
    const [files, setFiles] = useState([]);
    const [fileTypes, setFileTypes] = useState([]);
    const typeIconDir = window.initialState.typeIconDir || '';

    useEffect(() => {
        fetchFileTypes();
    }, []);

    const fetchFileTypes = async () => {
        const response = getFileTypesApi();
        setFileTypes(response.data.records);
    };

    const handleFileSelect = (event) => {
        setFiles([...files, ...event.target.files]);
    };

    const handleUpload = async () => {
        const formData = new FormData();
        files.forEach(file => formData.append('files', file));

        try {
            const response = postUploadDocsApi(formData);

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            
            onUpload();
        } catch (error) {
            console.error('Error:', error);
        }
    };
    
    const getIconFileName = (extension) => {
        const matchedType = fileTypes.find(ft => ft.extension === extension);
        if (matchedType == null){
            throw new Error('Can\'t handle file like this')
        }
        return `${typeIconDir}/${matchedType.iconFileName}`;
    }

    return (
        <div className="upload-popup">
            <div className="upload-header">
                <span>Upload Files</span>
                <button onClick={onClose}>X</button>
            </div>
            <div className="upload-body">
                <input type="file" multiple accept=".pdf" onChange={handleFileSelect} />
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
                                <img src={getIconFileName(file.type)} alt="file type"/>
                            </td>
                            <td>
                                <input type="text" defaultValue={file.name.replace(/\.[^/.]+$/, "_")}/>
                            </td>
                            <td>
                                <button>Preview</button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
            <div className="upload-footer">
                <button onClick={handleUpload}>Upload</button>
                <button onClick={onClose}>Cancel</button>
            </div>
        </div>
    );
};

export default UploadPopup;
