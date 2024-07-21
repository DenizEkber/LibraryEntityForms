import React from 'react';
import { NavLink } from 'react-router-dom';
import '../NavigationCss/NavBar.css'; // Create this file for styling

const NavBar = () => {
  return (
    <div className="navbar">
      <NavLink to="/dashboard" activeClassName="active">Dashboard</NavLink>
      <NavLink to="/books" activeClassName="active">Books</NavLink>
      <NavLink to="/library" activeClassName="active">Library</NavLink>
      <NavLink to="/teacher" activeClassName="active">Teacher</NavLink>
      <NavLink to="/students" activeClassName="active">Students</NavLink>
      <NavLink to="/authors" activeClassName="active">Authors</NavLink>
      <NavLink to="/settings" activeClassName="active">Settings</NavLink>
      <NavLink to="/signout" activeClassName="active">Sign Out</NavLink>
    </div>
  );
};

export default NavBar;
