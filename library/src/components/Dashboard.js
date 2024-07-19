import React from 'react';
import { Grid, Paper, Typography } from '@mui/material';
import BooksByCategoryChart from './BooksByCategoryChart';
import BooksByThemesChart from './BooksByThemesChart';
import OverdueChart from './OverdueChart';
import TeachersVsStudentsChart from './TeachersVsStudentsChart';

const Dashboard = () => {
  return (
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Typography variant="h4">Dashboard</Typography>
      </Grid>
      <Grid item xs={12} md={6}>
        <Paper>
          <Typography variant="h6">Books By Category</Typography>
          <BooksByCategoryChart />
        </Paper>
      </Grid>
      <Grid item xs={12} md={6}>
        <Paper>
          <Typography variant="h6">Books By Themes</Typography>
          <BooksByThemesChart />
        </Paper>
      </Grid>
      <Grid item xs={12} md={6}>
        <Paper>
          <Typography variant="h6">Overdue</Typography>
          <OverdueChart />
        </Paper>
      </Grid>
      <Grid item xs={12} md={6}>
        <Paper>
          <Typography variant="h6">Teachers vs Students</Typography>
          <TeachersVsStudentsChart />
        </Paper>
      </Grid>
    </Grid>
  );
};

export default Dashboard;
