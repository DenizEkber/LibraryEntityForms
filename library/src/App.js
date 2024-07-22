import React, { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import NavBar from './Navigation/NavBar';
import Dashboard from './Navigation/Dashboard';
import Books from './Navigation/Books';
import Libraries from './Navigation/Libraries';
import Teacher from './Navigation/Teacher';
import Student from './Navigation/Student';
import Author from './Navigation/Author';
import LoginPage from './Menu/LoginPage';
import RegisterPage from './Menu/RegisterPage';
import TopNav from './Navigation/TopNav';
import './App.css'; // Create this file for additional styling

const App = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);

  const handleLogin = (userData) => {
    setIsAuthenticated(true);
    setUser(userData);
  };

  return (
    <Router>
      <div className="app">
        <Routes>
          <Route path="/login" element={<LoginPage onLogin={handleLogin} />} />
          <Route path="/register" element={<RegisterPage />} />
          {isAuthenticated ? (
            <Route 
              path="/*"
              element={
                <>
                  <NavBar />
                  <div className="main-content">
                    <TopNav user={user} />
                    <div className="content">
                      <Routes>
                        <Route path="/dashboard" element={<Dashboard />} />
                        <Route path="/books" element={<Books />} />
                        <Route path="/library" element={<Libraries />} />
                        <Route path="/teacher" element={<Teacher />} />
                        <Route path="/students" element={<Student />} />
                        <Route path="/authors" element={<Author />} />
                        <Route path="/" element={<Navigate to="/dashboard" />} />
                      </Routes>
                    </div>
                  </div>
                </>
              }
            />
          ) : (
            <Route path="/*" element={<Navigate to="/login" />} />
          )}
        </Routes>
      </div>
    </Router>
  );
};

export default App;
