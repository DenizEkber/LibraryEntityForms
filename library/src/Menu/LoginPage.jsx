import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Auth.css'; // Create this file for additional styling

const LoginPage = ({ onLogin ,data}) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:8002/login', { email, password });
      if (response.data.success) {
        onLogin(response.data.data);
        
        navigate('/dashboard');
      } else {
        alert('Giriş başarısız. Lütfen bilgilerinizi kontrol edin.');
      }
    } catch (error) {
      console.error('Login error:', error);
      alert('Giriş yaparken bir hata oluştu.');
    }
  };

  return (
    <div className="auth-container">
      <h2>Giriş Yap</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label>Email:</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>
        <div className="input-group">
          <label>Şifre:</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <button type="submit">Giriş Yap</button>
        <div className="auth-footer">
          <p>Hesabınız yok mu? <a href="/register">Kayıt Olun</a></p>
        </div>
      </form>
    </div>
  );
};

export default LoginPage;
