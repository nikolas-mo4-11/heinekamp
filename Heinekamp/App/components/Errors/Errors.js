import React from 'react';
import './Errors.css';

const Errors = ({ errors }) => {
    return (
        <div className="errors">
            <div className="errors_text">
                {errors.join('\n')}
            </div>
        </div>
    );
};

export default Errors;
