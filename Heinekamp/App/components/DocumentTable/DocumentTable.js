import React from 'react';
import './DocumentTable.css';
import {Button, Space, Table} from 'antd';
const { Column } = Table;

const DocumentTable = ({ documents, setDocuments, onPreview, onDownload, onErrors }) => {
    const typeIconDir = window.initialState.typeIconDir || '';

    const getIconFileName = (name) => {
        return `${typeIconDir}/${name}`;
    }
    
    const formatCreatedDate = (createdDate) => {
        const date = new Date(createdDate);
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    }
    
    /*var testDocs = [ {
        fileType: { iconFileName: 'empty.svg'},
        name: 'test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test test ',
        createdDate: new Date(),
        downloadsCount: 12
    }];*/

    const paginationConfig = {
        pageSize: 20
    };

    return (
        <Table dataSource={documents} pagination={paginationConfig}>
            <Column
                key="icon"
                render={(_, record) => (
                    <Space size="middle">
                        <img className='icon' src={getIconFileName(record.fileType.iconFileName)} alt="file type"/>
                    </Space>
                    )}
            />
            <Column 
                title="Name" 
                dataIndex="name" 
                key="name"
            />
            <Column 
                title="Created Date"  
                key="createdDate" 
                render={(_, record) => (
                    <Space size="middle">
                        <div>{formatCreatedDate(record.createdDate)}</div>
                    </Space>
                )}
            />
            <Column title="Downloads Count" 
                    dataIndex="downloadsCount" 
                    key="downloadsCount"
            />
            <Column
                title="Actions"
                key="actions"
                render={(_, record) => (
                    <Space size="middle">
                        <Button onClick={() => onDownload(record)}>Download</Button>
                        <Button onClick={() => onPreview(record)}>Preview</Button>
                    </Space>
                )}
            />
        </Table>
    );
};

export default DocumentTable;
