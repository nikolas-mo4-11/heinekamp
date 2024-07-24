import React, {useEffect, useState} from 'react';
import './UploadPopup.css';
import {getFileTypesApi, postUploadDocsApi} from "../DocumentApi";
import {Button, Image, Modal, Space, Table, Upload} from "antd";
import Column from "antd/es/table/Column";
import {UploadOutlined} from "@ant-design/icons";

const UploadPopup = ({ onClose, onPreviewClick, onErrors, onUploadStart, onUploadFinish }) => {
    const typeIconDir = window.initialState.typeIconDir || '';
    
    const [fileTypes, setFileTypes] = useState([]);
    const [files, setFiles] = useState([]);
    
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
        setFiles([...event.fileList]);
    };

    const handleUpload = async () => {
        if (files.length === 0){
            onErrors(['Nothing uploaded. The list was empty'])
            onClose();
            return;
        }
        
        const formData = new FormData();
        files.forEach(file => formData.append('files', file.originFileObj));

        onClose();
        onUploadStart();
        await postUploadDocsApi(formData)
            .then(response => {
                if (!response.ok) {
                    onErrors(['Network response was not ok']);
                }
            })
            .then(_ => onUploadFinish())
            .catch(error => onErrors([error]));
    };
    
    const getIconFileName = (extension) => {
        const matchedType = fileTypes.find(ft => ft.extension === extension);
        if (matchedType == null){
            throw new Error(`Can't handle file of this type: ${extension}`)
        }
        return `${typeIconDir}/${matchedType.iconFileName}`;
    }

    const getFileExtension = (filename) => {
        return `.${filename.split('.').pop()}`; 
    };

    return (
        <div>
            <Modal title="Upload files"
                   open={true}
                   onCancel={onClose}
                   onOk={handleUpload}
                   width={800}
                   okText={'Upload'}>
                <Table dataSource={files} pagination={null}>
                    <Column
                        key="icon"
                        render={(_, record) => (
                            <Space size="middle">
                                <Image height={20} src={getIconFileName(getFileExtension(record.name))}/>
                            </Space>
                        )}
                    />
                    <Column
                        title="Name"
                        dataIndex="name"
                        key="name"
                    />
                    <Column
                        title="Actions"
                        key="actions"
                        render={(_, record) => (
                            <Space size="middle">
                                <Button onClick={() => onPreviewClick(record)}>Preview</Button>
                            </Space>
                        )}
                    />
                </Table>

                <Upload
                    name="file"
                    onChange={handleFileSelect}
                    marginTop={20}
                    accept='.doc, .docx, .pdf, image/png, image/jpeg, image/jpg, .xls, .xlsx, .txt, .gif'
                    multiple={true}
                >
                    <Button icon={<UploadOutlined/>}>Upload files</Button>
                </Upload>
            </Modal>
        </div>
    );
};

export default UploadPopup;
