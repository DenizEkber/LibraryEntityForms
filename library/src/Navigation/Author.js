import React, { useEffect, useState } from 'react';
import axios from 'axios';
import '../NavigationCss/Author.css'; // Özel CSS dosyası (isteğe bağlı)

const defaultProfileImage = '/profileImage.jpg'; // Varsayılan resim yolu

const Author = () => {
  const [authors, setAuthors] = useState([]);

  useEffect(() => {
    const fetchAuthors = async () => {
      try {
        const response = await axios.get('http://localhost:8002/authors'); // API endpoint
        const { data } = response.data;
        setAuthors(data || []);
      } catch (error) {
        console.error('Error fetching authors:', error);
      }
    };

    fetchAuthors();
  }, []);

  return (
    <div className="author-container">
      <h1>Authors</h1>
      <div className="author-list">
        {authors.map((author, index) => (
          <div key={index} className="author-card">
            <div className="author-image">
              {author.PhotoData ? (
                <img
                  src={`data:image/jpeg;base64,${author.PhotoData}`}
                  alt={`${author.FirstName} ${author.LastName}`}
                  className="author-thumbnail"
                />
              ) : (
                <img
                  src={defaultProfileImage}
                  alt="Default"
                  className="author-thumbnail"
                />
              )}
            </div>
            <div className="author-info">
              <strong>{author.FirstName} {author.LastName}</strong>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Author;
