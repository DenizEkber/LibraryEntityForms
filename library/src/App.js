/*import React from 'react';
import { Grid, Paper } from '@mui/material';
import Sidebar from './components/Sidebar';
import Dashboard from './components/Dashboard';

const App = () => {
  return (
    <Grid container>
      <Grid item xs={2}>
        <Paper style={{height:"100vh"}}>
          <Sidebar />
        </Paper>
      </Grid>
      <Grid item xs={10}>
        <Dashboard />
      </Grid>
    </Grid>
  );
};

export default App;*/


import React from 'react';
import { BrowserRouter as Router, Route, Switch, Routes } from 'react-router-dom';
import NavBar from './Navigation/NavBar';
import Dashboard from './Navigation/Dashboard';
/*import Books from './pages/Books';
import Library from './pages/Library';
import Teacher from './pages/Teacher';
import Students from './pages/Students';
import Authors from './pages/Authors';
import Settings from './pages/Settings';
import SignOut from './pages/SignOut';*/
import './App.css'; // Create this file for additional styling
import TopNav from './Navigation/TopNav';

const App = () => {
  return (
    <Router>
      <div className="app">
      <NavBar />
        <div className="main-content">
          <TopNav />
          <div className="content">
            <Routes>
              <Route path="/dashboard" element={<Dashboard />} />
              {/*<Route path="/books" element={<Books />} />
              <Route path="/library" element={<Library />} />
              <Route path="/teacher" element={<Teacher />} />
              <Route path="/students" element={<Students />} />
              <Route path="/authors" element={<Authors />} />
              <Route path="/settings" element={<Settings />} />
              <Route path="/signout" element={<SignOut />} />*/}
            </Routes>
          </div>
        </div>
      </div>
    </Router>
  );
};

export default App;
