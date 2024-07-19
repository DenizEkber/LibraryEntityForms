import React from 'react';
import { Chart, ArcElement, Tooltip, Legend } from 'chart.js';
import { Doughnut } from 'react-chartjs-2';

Chart.register(ArcElement, Tooltip, Legend);

const data = {
  labels: ['Theme A', 'Theme B', 'Theme C', 'Theme D', 'Theme E'],
  datasets: [
    {
      label: 'Books By Themes',
      data: [10, 20, 30, 25, 15],
      backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF'],
    },
  ],
};

const BooksByThemesChart = () => {
  return <Doughnut data={data} />;
};

export default BooksByThemesChart;
