import React from 'react';
import '../NavigationCss/UserModel.css'; // Create this file for additional styling

const UserModal = ({ user, onClose }) => {
  console.log(user)
  return (
    <div className="modal">
      <div className="modal-content">
        <span className="close" onClick={onClose}>&times;</span>
        <h2>User Information</h2>
        <table>
          <tbody>
            <tr>
              <td>First Name:</td>
              <td>{user.FirstName}</td>
            </tr>
            <tr>
              <td>Last Name:</td>
              <td>{user.LastName}</td>
            </tr>
            <tr>
              <td>Email:</td>
              <td>{user.Email}</td>
            </tr>
            <tr>
              <td>Role:</td>
              <td>{user.Role}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default UserModal;
