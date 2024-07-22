import React, { useEffect, useState } from 'react';
import axios from 'axios';
import '../NavigationCss/Student.css'; // Özel CSS dosyası (isteğe bağlı)

const defaultProfileImage = '/profileImage.jpg'; // Varsayılan resim yolu

const Student = () => {
  const [students, setStudents] = useState([]);

  useEffect(() => {
    const fetchStudents = async () => {
      try {
        const response = await axios.get('http://localhost:8002/students'); // API endpoint
        const { data } = response.data;
        setStudents(data || []);
      } catch (error) {
        console.error('Error fetching students:', error);
      }
    };

    fetchStudents();
  }, []);

  return (
    <div className="student-container">
      <h1>Students</h1>
      <div className="student-list">
        {students.map((student, index) => (
          <div key={index} className="student-card">
            <div className="student-image">
              {student.PhotoData ? (
                <img
                  src={`data:image/jpeg;base64,${student.PhotoData}`}
                  alt={`${student.FirstName} ${student.LastName}`}
                  className="student-thumbnail"
                />
              ) : (
                <img
                  src={defaultProfileImage}
                  alt="Default"
                  className="student-thumbnail"
                />
              )}
            </div>
            <div className="student-info">
              <strong>{student.FirstName} {student.LastName}</strong>
              <p>Term: {student.Term || 'Unknown'}</p>
              <p>Group: {student.Group || 'Unknown'}</p>
              <p>Faculty: {student.Faculty || 'Unknown'}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Student;
