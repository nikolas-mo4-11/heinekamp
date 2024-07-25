import React from 'react';
import './DocumentTable.css';
import {Button, Space, Table} from 'antd';
const { Column } = Table;

const DocumentTable = ({ documents, onPreview, onDownload, onErrors, onCreateLink, selectedRowKeys, setSelectedRowKeys }) => {
    const typeIconDir = window.initialState.typeIconDir || '';
    
    const getIconFileName = (name) => {
        return `${typeIconDir}/${name}`;
    }
    
    const formatCreatedDate = (createdDate) => {
        const date = new Date(createdDate);
        return `${date.toLocaleDateString()} ${date.toLocaleTimeString()}`;
    }

    const onSelectChange = (newSelectedRowKeys) => {
        setSelectedRowKeys(newSelectedRowKeys);
    };

    const rowSelection = {
        selectedRowKeys,
        onChange: onSelectChange,
    };
    
    /*var testDocs = [ {
        fileType: { iconFileName: 'empty.svg'},
        name: 'test test test test test test test test test test test test',
        createdDate: new Date(),
        downloadsCount: 12
    }];*/

    const paginationConfig = {
        pageSize: 20
    };

    return (
        <Table dataSource={documents} rowKey="id" pagination={paginationConfig} rowSelection={rowSelection}>
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
                key="name"
                render={(_, record) => (
                    <Space size="middle">
                        {`${record.name}${record.fileType.extension}`}
                    </Space>
                )}
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
                        <Button onClick={() => onCreateLink(record)}>Create download link</Button>
                    </Space>
                )}
            />
        </Table>
    );
};

export default DocumentTable;
