import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import './Auth.css'; // Create this file for additional styling

const RegisterPage = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      alert('Şifreler eşleşmiyor.');
      return;
    }
    try {
      await axios.post('http://localhost:8002/register', { email, password });
      navigate('/login');
    } catch (error) {
      console.error('Registration error:', error);
      alert('Kayıt işlemi sırasında bir hata oluştu.');
    }
  };

  return (
    <div className="auth-container">
      <h2>Kayıt Ol</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-group">
          <label>Email:</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>
        <div className="input-group">
          <label>Şifre:</label>
          <input type="password" value={password} onChange={(e) => setPassword(e.target.value)} required />
        </div>
        <div className="input-group">
          <label>Şifreyi Doğrulayın:</label>
          <input type="password" value={confirmPassword} onChange={(e) => setConfirmPassword(e.target.value)} required />
        </div>
        <button type="submit">Kayıt Ol</button>
        <div className="auth-footer">
          <p>Hesabınız var mı? <a href="/login">Giriş Yapın</a></p>
        </div>
      </form>
    </div>
  );
};

export default RegisterPage;
