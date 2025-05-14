import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import './App.css';

// Import components
import Layout from './components/Layout';
import Home from './components/Home';
import Login from './components/Login';
import Register from './components/Register';
import ProtectedRoute from './components/ProtectedRoute';

// Import context providers
import { AuthProvider } from './contexts/AuthContext';

// Placeholder components for protected routes
const Decks = () => <div className="p-4">Decks Page (Coming Soon)</div>;
const CreateDeck = () => <div className="p-4">Create Deck Page (Coming Soon)</div>;
const Profile = () => <div className="p-4">Profile Page (Coming Soon)</div>;
const NotFound = () => <div className="p-4 text-center"><h1 className="text-4xl">404 - Not Found</h1></div>;

const App: React.FC = () => {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            {/* Public Routes */}
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            
            {/* Protected Routes */}
            <Route element={<ProtectedRoute />}>
              <Route path="/decks" element={<Decks />} />
              <Route path="/create-deck" element={<CreateDeck />} />
              <Route path="/profile" element={<Profile />} />
            </Route>
            
            {/* 404 and fallback routes */}
            <Route path="/404" element={<NotFound />} />
            <Route path="*" element={<Navigate to="/404" replace />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
};

export default App;
