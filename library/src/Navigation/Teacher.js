import React, { useEffect, useState } from 'react';
import axios from 'axios';
import '../NavigationCss/Teacher.css'; // Özel CSS dosyası (isteğe bağlı)

const defaultProfileImage = '/profileImage.jpg'; // Varsayılan resim yolu

const Teacher = () => {
  const [teachers, setTeachers] = useState([]);

  useEffect(() => {
    const fetchTeachers = async () => {
      try {
        const response = await axios.get('http://localhost:8002/teachers'); // API endpoint
        const { data } = response.data;
        setTeachers(data || []);
      } catch (error) {
        console.error('Error fetching teachers:', error);
      }
    };

    fetchTeachers();
  }, []);

  return (
    <div className="teacher-container">
      <h1>Teachers</h1>
      <div className="teacher-list">
        {teachers.map((teacher, index) => (
          <div key={index} className="teacher-card">
            <div className="teacher-image">
              {teacher.PhotoData ? (
                <img
                  src={`data:image/jpeg;base64,${teacher.PhotoData}`}
                  alt={`${teacher.FirstName} ${teacher.LastName}`}
                  className="teacher-thumbnail"
                />
              ) : (
                <img
                  src={defaultProfileImage}
                  alt="Default"
                  className="teacher-thumbnail"
                />
              )}
            </div>
            <div className="teacher-info">
              <strong>{teacher.FirstName} {teacher.LastName}</strong>
              <p>Department: {teacher.Department || 'Unknown'}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Teacher;
