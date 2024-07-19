import React from 'react';
import { Grid, Paper } from '@mui/material';
import Sidebar from './components/Sidebar';
import Dashboard from './components/Dashboard';

const App = () => {
  return (
    <Grid container>
      <Grid item xs={2}>
        <Paper>
          <Sidebar />
        </Paper>
      </Grid>
      <Grid item xs={10}>
        <Dashboard />
      </Grid>
    </Grid>
  );
};

export default App;
