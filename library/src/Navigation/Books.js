import React, { useEffect, useState } from 'react';
import '../NavigationCss/Books.css'; // Import the CSS file
import axios from 'axios';

const Books = () => {
  const [books, setBooks] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get('http://localhost:8002/books');
        const { data } = response.data;

        // data.books dizisini kontrol edin
        setBooks(data || []);
        console.log(data); // Tüm veriyi kontrol edin
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchData();
  }, []);

  return (
    <div className="books-container">
      <h1>Books</h1>
      <ul className="books-list">
        {books.map((book, index) => (
          <li key={index}>
            <div className="book-image">
              {book.PhotoData ? (
                <img
                  src={`data:image/jpeg;base64,${book.PhotoData}`}
                  alt={book.BookName}
                  className="book-thumbnail"
                />
              ) : (
                <img
                  src="path_to_default_image" // Fallback image path
                  alt="Default"
                  className="book-thumbnail"
                />
              )}
            </div>
            <div className="book-info">
              <strong>{book.BookName}</strong>
              <p>by {book.AuthorFirstName} {book.AuthorLastName}</p>
              <p>{book.BookComment}</p>
              <div className="book-details">
                <span>{book.BookPages} pages</span>
                <span>Published in {book.BookYearPress}</span>
                <span>Category: {book.CategoryName || 'Unknown'}</span>
                <span>Theme: {book.ThemeName || 'Unknown'}</span>
                <span>Press: {book.PressName || 'Unknown'}</span>
              </div>
              <p className="book-quantity">Quantity: {book.BookQuantity}</p>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Books;
