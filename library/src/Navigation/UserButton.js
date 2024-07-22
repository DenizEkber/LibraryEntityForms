import React, { useState } from 'react';
import UserModal from './UserModal';
import '../NavigationCss/TopNav.css';

const TopNav = ({ user }) => {
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleUserButtonClick = () => {
    setIsModalOpen(true);
  };

  const handleCloseModal = () => {
    setIsModalOpen(false);
  };

  return (
    <div className="top-nav">
      {user ? (
        <>
          <button className="user-button" onClick={handleUserButtonClick}>
            {user.userName || "User"}
          </button>
          {isModalOpen && <UserModal user={user} onClose={handleCloseModal} />}
        </>
      ) : (
        <button className="user-button" onClick={handleUserButtonClick}>
          {"Guest"}
        </button>
      )}
    </div>
  );
};

export default TopNav;
