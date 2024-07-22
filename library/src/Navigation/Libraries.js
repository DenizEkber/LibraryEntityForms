import React, { useEffect, useState } from 'react';
import axios from 'axios';
import '../NavigationCss/Libraries.css'; // CSS dosyasını ekleyin

const Libraries = () => {
  const [libraries, setLibraries] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('http://localhost:8002/libraries');
        const { data } = response.data;

        // Set libraries state with the data
        setLibraries(data || []);
        console.log(data); // Veriyi kontrol edin
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="libraries-container">
      <h1>Libraries</h1>
      <div className="libraries-grid">
        {libraries.map((library, index) => (
          <div className="library-card" key={index}>
            <div className="library-image">
              {library.PhotoData ? (
                <img
                  src={`data:image/jpeg;base64,${library.PhotoData}`}
                  alt={library.Name}
                  className="library-thumbnail"
                />
              ) : (
                <img
                  src="path_to_default_image" // Fallback image path
                  alt="Default"
                  className="library-thumbnail"
                />
              )}
            </div>
            <div className="library-info">
              <h2>{library.Name}</h2>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Libraries;
