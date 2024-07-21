import React from 'react';
import './Header.css';

const Header = ({ onUploadClick }) => {
    return (
        <header className="header">
            <div className="header-logo">
                <img src="./assets/drheinekamp_logo.svg" alt="Organization Logo" />
            </div>
            <div className="header-menu">
                <button onClick={onUploadClick}>Upload</button>
            </div>
        </header>
    );
};

export default Header;
