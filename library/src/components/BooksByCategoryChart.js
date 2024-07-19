import React from 'react';
import { Chart, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';

Chart.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const data = {
  labels: ['Fiction', 'Fantasy', 'Mystery', 'Romance', 'Science', 'Drama', 'Education'],
  datasets: [
    {
      label: 'Books By Category',
      data: [20000, 15000, 10000, 5000, 7000, 20000, 25000],
      backgroundColor: 'rgba(75, 192, 192, 0.6)',
    },
  ],
};

const BooksByCategoryChart = () => {
  return <Bar data={data} />;
};

export default BooksByCategoryChart;
