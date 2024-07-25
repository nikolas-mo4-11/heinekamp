import React, { useState } from 'react';
import './PreviewPopup.css';
import {Button, Col, Modal, Row, Image, Card, Input} from 'antd';
import Meta from "antd/es/card/Meta";

const PreviewPopup = ({ document, onClose, onDeleteDoc, onUpdateDoc }) => {
    const typeIconDir = window.initialState.typeIconDir || '';
    const previewStorageDir = window.initialState.previewStorageDir || '';
    
    const [isInEditMode, setIsInEditMode] = useState(false);
    const [newName, setNewName] = useState(document.name);
    
    const onEditSave = () => {
        if (isInEditMode){
            onUpdateDoc({
                id: document.id,
                name: newName,
                downloadsCount: document.downloadsCount,
            });
            document.name = newName;
        }
        setIsInEditMode(!isInEditMode);
    }

    return (
        <div className='base-container'>
            <Modal 
                title="Preview" 
                open={true} 
                onCancel={onClose} 
                width={800}
                footer={[
                    <Button key="back" onClick={onClose}>
                        Close
                    </Button>,
                    <Button key="delete" onClick={() => onDeleteDoc(document)}>
                        Delete
                    </Button>,
                    <Button
                        key="editSave" type="primary" onClick={onEditSave}
                    >
                        {isInEditMode ? 'Save' : 'Edit'}
                    </Button>,
                ]}>
                <Row>
                    <Col span={24}>
                        <div style={{display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: 20}}>
                            <Image height={40} src={`${typeIconDir}/${document.fileType.iconFileName}`}/>
                        </div>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        
                        <div>
                            {isInEditMode
                                ? (<Input
                                    value={newName}
                                    onChange={(e) => setNewName(e.target.value)}/>)
                                : <div className='title'>{document.name}</div>
                            }
                        </div>
                    </Col>
                </Row>
                <Row>
                    <Col span={12}>
                        <Card>
                            <Meta
                                title="Created date"
                                description={new Date(document.createdDate).toLocaleDateString()}
                            />
                        </Card>
                    </Col>
                    <Col span={12}>
                        <Card>
                            <Meta
                                title="Downloads"
                                description={document.downloadsCount.toString()}
                            />
                        </Card>
                    </Col>
                </Row>
                <Row>
                    <Col span={24}>
                        <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: 20 }}>
                            <Image height={500} src={`${previewStorageDir}/${document.id}.png`} alt="preview"/>
                        </div>
                    </Col>
                </Row>
            </Modal>
        </div>
    );
};

export default PreviewPopup;
