import React from 'react';
import { Chart, LineElement, PointElement, LineController, CategoryScale, LinearScale, Title, Tooltip, Legend } from 'chart.js';
import { Line } from 'react-chartjs-2';

Chart.register(LineElement, PointElement, LineController, CategoryScale, LinearScale, Title, Tooltip, Legend);

const data = {
  labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
  datasets: [
    {
      label: 'Teachers',
      data: [10, 12, 8, 14, 10, 15, 9],
      borderColor: 'rgba(75, 192, 192, 1)',
      fill: false,
    },
    {
      label: 'Students',
      data: [8, 9, 6, 10, 7, 11, 8],
      borderColor: 'rgba(153, 102, 255, 1)',
      fill: false,
    },
  ],
};

const OverdueChart = () => {
  return <Line data={data} />;
};

export default OverdueChart;
