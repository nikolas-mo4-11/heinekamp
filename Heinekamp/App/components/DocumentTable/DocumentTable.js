import React, { useState, useEffect } from 'react';
import './DocumentTable.css';
import Pagination from "../Pagination/Pagination";
import {getPageApi} from "../DocumentApi";

const DocumentTable = ({ onPreviewClick, onErrors, onDownload }) => {
    const [documents, setDocuments] = useState([]);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const documentStorageDir = window.initialState.documentStorageDir || '';
    const typeIconDir = window.initialState.typeIconDir || '';

    useEffect( () => {
        fetchDocuments();
    }, [page]);

    const fetchDocuments = () => {
        getPageApi(page)
            .then(response => response.json())
            .then(response => {
                setDocuments(response.records);
                setTotalPages(response.totalPagesCount);
            })
            .catch(error => onErrors(error));
    };

    const handleDownload = (doc) => {
        const ext = doc.fileType.extension;
        const link = document.createElement('a');
        link.href = `${documentStorageDir}/${doc.id}${ext}`;
        link.download = `${doc.name}${ext}`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        
        onDownload(doc);
    };

    const getIconFileName = (name) => {
        return `${typeIconDir}/${name}`;
    }
    
    const formatCreatedDate = (createdDate) => {
        const date = new Date(createdDate);
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    }

    return (
        <div className="document-table">
            <table>
                <thead>
                <tr>
                    <th>Name</th>
                    <th>Created Date</th>
                    <th>Downloads Count</th>
                    <th>Download</th>
                    <th>Preview</th>
                </tr>
                </thead>
                <tbody>
                {documents.map(doc => (
                    <tr key={doc.id}>
                        <td>
                            <div>
                                <img className='icon' src={getIconFileName(doc.fileType.iconFileName)} alt="file type"/>
                                {doc.name}
                            </div>

                        </td>
                        <td>{formatCreatedDate(doc.createdDate)}</td>
                        <td>{doc.downloadsCount}</td>
                        <td>
                            <button onClick={() => handleDownload(doc)}>Download</button>
                        </td>
                        <td>
                            <button onClick={() => onPreviewClick(doc)}>Preview</button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
            <Pagination currentPage={page} totalPages={totalPages} onPageChange={setPage} />
        </div>
    );
};

export default DocumentTable;
