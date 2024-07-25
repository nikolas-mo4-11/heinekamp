import React, { useState } from 'react';
import './LinkPopup.css';
import {Button, Col, Modal, Row, Image, Input, Select} from 'antd';
import {postCreateLinkApi} from "../DocumentApi";

const LinkPopup = ({ document, onClose, onErrors }) => {
    const typeIconDir = window.initialState.typeIconDir || '';
    
    const [count, setCount] = useState('');
    const [unit, setUnit] = useState('minutes');
    const [link, setLink] = useState(null);
    const [expires, setExpires] = useState(new Date())
    
    
    const onCreate = () => {
        if (count === ''){
            onErrors(['Period is not set']);
        }

        postCreateLinkApi(document.id, expires.toISOString())
            .then(response => response.json())
            .then(response =>{
                setLink(response.link);
                console.log(response);
            }) 
            .catch(error => onErrors([error]));
    }
    
    const getExpirationDate = (countStr, unit) => {
        const date = new Date();
        const countNum = parseInt(countStr, 10);
        
        switch (unit) {
            case 'minutes':
                date.setMinutes(date.getMinutes() + countNum);
                break;
            case 'hours':
                date.setHours(date.getHours() + countNum);
                break;
            case 'days':
                date.setDate(date.getDate() + countNum);
                break;
            case 'months':
                date.setMonth(date.getMonth() + countNum);
                break;
            default:
                throw new Error('Unsupported unit');
        }
        
        return date;
    }
    
    const closeButton = <Button key="back" onClick={onClose}>Close</Button>;
    const createButton = <Button disabled={count === '0' || count === ''} type="primary" key="create" onClick={onCreate}>Create link</Button>;
    const okButton = <Button key="ok" type="primary" onClick={onClose}>Ok</Button>;
    
    const handleInputChange = (e) => {
        const newValue = e.target.value;
        if (newValue === '' || /^[0-9\b]+$/.test(newValue)) {
            setExpires(getExpirationDate(newValue, unit));
            setCount(newValue);
        }
    };
    
    const handleSelectChange = (value) => {
        setExpires(getExpirationDate(count, value));
        setUnit(value);
    }

    return (
        <div className='base-container'>
            <Modal
                title="Create link"
                open={true}
                onCancel={onClose}
                width={800}
                footer={link != null ? [okButton] : [closeButton, createButton]}
            >
                <Row><Col span={24}>
                        <div style={{display: 'flex', justifyContent: 'center', alignItems: 'center', marginTop: 20}}>
                            <Image height={40} src={`${typeIconDir}/${document.fileType.iconFileName}`}/>
                        </div>
                </Col></Row>
                <Row><Col span={24}>
                    <div className='title'>{document.name}</div>
                </Col></Row>
                
                How long should the document be available by the link?
                <Row>
                    <Input
                        value={count}
                        onChange={handleInputChange}
                        maxLength={6}
                        style={{ width: 120 }}
                    />
                    <Select
                        defaultValue="minutes"
                        style={{ width: 120 }}
                        onChange={handleSelectChange}
                        options={[
                            {value: 'minutes', label: 'minutes',},
                            {value: 'hours', label: 'hours',},
                            {value: 'days', label: 'days',},
                            {value: 'months', label: 'months',},
                        ]}
                    />
                </Row>
                
                Link will be available till {expires.toLocaleDateString()} {expires.toLocaleTimeString()}
                
                {link != null && <div>
                    <Row><Col span={24}>
                        Please, copy the link before closing the popup
                    </Col></Row>
                    <Row><Col span={24}>
                        <div style={{fontSize: 14, fontWeight: 16}}>
                            {link}
                        </div>
                    </Col></Row>
                </div> }
                
            </Modal>
        </div>
    );
};

export default LinkPopup;