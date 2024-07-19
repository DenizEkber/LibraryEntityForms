import React from 'react';
import { Chart, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';

Chart.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

const data = {
  labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
  datasets: [
    {
      label: 'Teachers',
      data: [3000, 4000, 3500, 5000, 4500, 5500, 6000],
      backgroundColor: 'rgba(75, 192, 192, 0.6)',
    },
    {
      label: 'Students',
      data: [5000, 6000, 7000, 8000, 7500, 8500, 9000],
      backgroundColor: 'rgba(153, 102, 255, 0.6)',
    },
  ],
};

const TeachersVsStudentsChart = () => {
  return <Bar data={data} />;
};

export default TeachersVsStudentsChart;
