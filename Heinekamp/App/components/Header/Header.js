import React from 'react';
import './Header.css';
import { Button } from 'antd';

const Header = ({ onUpload }) => {
    return (
        <header className="header">
            <div className="header-logo">
                <img src="./assets/drheinekamp_logo.png" alt="Organization Logo" />
            </div>
            <div className="header-menu">
                <Button onClick={onUpload}>Upload</Button>
            </div>
        </header>
    );
};

export default Header;
