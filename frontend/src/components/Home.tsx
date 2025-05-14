import React from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Home: React.FC = () => {
  const { isAuthenticated, user } = useAuth();

  return (
    <div className="flex flex-col items-center">
      <div className="text-center mb-12">
        <h1 className="text-4xl font-bold text-blue-700 mb-4">
          Welcome to FlipCard App
        </h1>
        <p className="text-xl text-gray-600 max-w-2xl mx-auto">
          The most effective way to learn and memorize with smart flashcards
        </p>
      </div>

      {isAuthenticated ? (
        <div className="text-center">
          <h2 className="text-2xl font-semibold mb-6">
            Welcome back, {user?.username}!
          </h2>
          <div className="space-y-4">
            <div>
              <Link 
                to="/decks" 
                className="inline-block bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg shadow-md transition"
              >
                Go to My Decks
              </Link>
            </div>
            <div>
              <Link 
                to="/create-deck" 
                className="inline-block bg-green-600 hover:bg-green-700 text-white font-bold py-3 px-6 rounded-lg shadow-md transition"
              >
                Create New Deck
              </Link>
            </div>
          </div>
        </div>
      ) : (
        <div className="text-center">
          <h2 className="text-2xl font-semibold mb-6">
            Join thousands of students improving their memory
          </h2>
          <div className="space-x-4">
            <Link 
              to="/register" 
              className="inline-block bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-6 rounded-lg shadow-md transition"
            >
              Sign Up
            </Link>
            <Link 
              to="/login" 
              className="inline-block bg-gray-200 hover:bg-gray-300 text-gray-800 font-bold py-3 px-6 rounded-lg shadow-md transition"
            >
              Login
            </Link>
          </div>
        </div>
      )}

      {/* Features section */}
      <div className="mt-20 grid md:grid-cols-3 gap-8 w-full max-w-6xl">
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-xl font-semibold text-blue-600 mb-3">Create Flashcards</h3>
          <p className="text-gray-700">
            Make custom flashcards with rich text, images, and formatting to help you learn efficiently.
          </p>
        </div>
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-xl font-semibold text-blue-600 mb-3">Spaced Repetition</h3>
          <p className="text-gray-700">
            Our smart algorithm helps you review cards at the optimal time to maximize retention.
          </p>
        </div>
        <div className="bg-white p-6 rounded-lg shadow-md">
          <h3 className="text-xl font-semibold text-blue-600 mb-3">Track Progress</h3>
          <p className="text-gray-700">
            Set goals and track your progress with detailed statistics and insights.
          </p>
        </div>
      </div>
    </div>
  );
};

export default Home;
