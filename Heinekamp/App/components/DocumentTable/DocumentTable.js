import React, { useState, useEffect } from 'react';
import './DocumentTable.css';
import Pagination from "../Pagination/Pagination";

const DocumentTable = ({ onPreviewClick }) => {
    const [documents, setDocuments] = useState([]);
    const [page, setPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const documentStorageDir = window.initialState.documentStorageDir || '';

    useEffect(() => {
        fetchDocuments();
    }, [page]);

    const fetchDocuments = async () => {
        const response = getPageApi(page);
        setDocuments(response.data.records);
        setTotalPages(response.data.totalPagesCount);
    };

    const handleDownload = (fileName) => {
        // Формируем URL для скачивания файла
        const fileUrl = `${documentStorageDir}/${fileName}`;
        const link = document.createElement('a');
        link.href = fileUrl;
        link.download = fileName; // Имя файла для сохранения
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    };

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
                        <td>{doc.name}</td>
                        <td>{new Date(doc.createdDate).toLocaleDateString()}</td>
                        <td>{doc.downloadsCount}</td>
                        <td>
                            <button onClick={() => handleDownload(`${doc.id}.${doc.fileType.extension}`)}>Download</button>
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
