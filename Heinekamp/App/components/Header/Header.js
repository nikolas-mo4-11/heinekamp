import React from 'react';
import './Header.css';
import { Button } from 'antd';

const Header = ({ onUpload, selectedRowKeys, onDownloadMany }) => {
    return (
        <header className="header">
            <div className="header-logo">
                <img src="./assets/drheinekamp_logo.png" alt="Organization Logo" />
            </div>
            <div className="header-menu">
                <Button onClick={onDownloadMany} hidden={selectedRowKeys.length === 0} type="primary">Download selected</Button>
                <Button onClick={onUpload}>Upload</Button>
            </div>
        </header>
    );
};

export default Header;
