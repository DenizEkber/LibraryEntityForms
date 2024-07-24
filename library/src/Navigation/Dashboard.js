import React, { useState, useEffect } from 'react';
import axios from 'axios';
import '../NavigationCss/Dashboard.css';
import { PieChart, Pie, Cell, BarChart, Bar, XAxis, YAxis, LineChart, Line, Tooltip, CartesianGrid , Legend  } from 'recharts';

const Dashboard = () => {
  const [mostReadBooks, setMostReadBooks] = useState([]);
  const [themesData, setThemesData] = useState([]);
  const [categoryData, setCategoryData] = useState([]);
  const [overdueData, setOverdueData] = useState([]);
  const [teacherStudentData, setTeacherStudentData] = useState([]);
  const [totalTeachers, setTotalTeachers] = useState(0);
  const [totalStudents, setTotalStudents] = useState(0);

  //const COLORS = ['#5D5FEF', '#00C49F', '#FFBB28', '#FF8042', '#8884D8', '#8DD1E1', '#82CA9D'];
  const mostbookColor = ['#FFE2E5', '#FFF4DE', '#DCFCE7', '#F3E8FF']

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('http://localhost:8002/dashboard');
        const { data } = response.data;

        setMostReadBooks(data.mostReadBook || []);
        setThemesData(data.readBookThemes || []);
        setCategoryData(data.readBookCategory || []);
        setOverdueData(data.overDue || []);
        setTeacherStudentData(data.teacherVsStudent || []);


        
        // Hesaplama
        const teachersCount = data.teacherVsStudent.reduce((sum, item) => sum + item.Item4, 0);
        const studentsCount = data.teacherVsStudent.reduce((sum, item) => sum + item.Item5, 0);

        setTotalTeachers(teachersCount);
        setTotalStudents(studentsCount);

      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };
    const totalThemesUsage = themesData.reduce((sum, item) => sum + item.Item3, 0);
    fetchData();
  }, []);

  const totalThemesUsage = themesData.reduce((sum, item) => sum + item.Item3, 0);

  return (
    <div className="dashboard">
      <div className="summary-cards">
        {mostReadBooks.length > 0 ? mostReadBooks.map((entry, index) => (
          <div className="card" style={{background:mostbookColor[index % mostbookColor.length]}} key={index}>
            <h2>{entry.Item2}</h2>
            <p>{entry.Item1}</p>
          </div>
        )) : <p>No data available</p>}
      </div>
      <div className="charts-row">
        <div className="chart-container">
          <h3>Books By Themes</h3>
          <PieChart width={400} height={400}>
          <Pie
            data={themesData}
            cx={200}
            cy={200}
            innerRadius={80} // Donut efektini oluşturur
            outerRadius={150}
            fill="#8884d8"
            dataKey="Item2" // Pie'daki değerler
            nameKey="Item1" // Legend'daki isimler
            labelLine={false}
            label={({ cx, cy, midAngle, innerRadius, outerRadius, value, payload }) => {
              const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
              const x = cx + radius * Math.cos(-midAngle * Math.PI / 180);
              const y = cy + radius * Math.sin(-midAngle * Math.PI / 180);
              
              const percent = ((value / totalThemesUsage) * 100).toFixed(2);
              return (
                <text x={x} y={y} fill="white" textAnchor="middle" dominantBaseline="central">
                  {`${percent}%`}
                </text>
              );
            }}
          >
            {/*themesData.map((entry, index) => (
              <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
            ))*/}
          </Pie>
          <Tooltip />
          <Legend />
        </PieChart>
        </div>
        <div className="chart-container">
          <h3>Books By Category</h3>
          <BarChart width={600} height={300} data={categoryData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="Item1" />
            <YAxis />
            <Tooltip />
            <Bar dataKey="Item2" name="Count" fill="#0095FF" />
          </BarChart>
        </div>
      </div>
      <div className="charts-row">
        <div className="chart-container">
          <h3>Overdue</h3>
          <LineChart width={600} height={300} data={overdueData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis 
              dataKey="Item1"
              tickFormatter={(tick) => new Date(tick).toLocaleDateString()}
            />
            <YAxis />
            <Tooltip />
            <Line type="monotone" dataKey="Item2" stroke="#07E098" name="Student" />
            <Line type="monotone" dataKey="Item3" stroke="#0095FF" name="Teacher" />
          </LineChart>
        </div>
        <div className="chart-container">
          <h3>Teachers vs Students</h3>
          <BarChart width={600} height={300} data={teacherStudentData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis 
              dataKey="Item1"
              tickFormatter={(tick) => new Date(tick).toLocaleDateString()}
            />
            <YAxis />
            <Tooltip />
            <Bar dataKey="Item2" name="Teachers" fill="#4AB58E" />
            <Bar dataKey="Item3" name="Students" fill="#FFCF00" />
          </BarChart>
          <div className="totals">
            <div style={{ color: "#4AB58E" }}>Teacher Total: {totalTeachers}</div>
             <div style={{ color: "#FFCF00" }}>Student Total: {totalStudents}</div>
        </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
