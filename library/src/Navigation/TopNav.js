import React from 'react';
import '../NavigationCss/TopNav.css';

const TopNav = () => {
  return (
    <div className="topnav">
      <h1>EagleVision</h1>
      <div className="user-info">
        <img src="user-icon.png" alt="User Icon" className="user-icon" />
        <div className="user-details">
          <span>User</span>
          <span className="user-role">Admin</span>
        </div>
      </div>
    </div>
  );
};

export default TopNav;
