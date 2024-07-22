import React from 'react';
import UserButton from './UserButton';

const TopNav = ({ user }) => {
  console.log(user)
  return (
    <div className="topnav">
      <h1>Library Management System</h1>
      <UserButton user={user} />
    </div>
  );
};

export default TopNav;
