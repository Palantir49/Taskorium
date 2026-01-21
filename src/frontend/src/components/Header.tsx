import React from 'react';
import { HeaderProps } from '../types';
import {FaChartLine, FaFileAlt, FaSignInAlt, FaSignOutAlt, FaUserCircle} from 'react-icons/fa';
import './Header.css';

function Header({activeTab, onTabChange, authInfo}:HeaderProps) {
    const {isAuthenticated, userFullName, onLogin, onLogout} = authInfo || {};

    return (
        <header className="header">
            <div className="header-content">
                <div className="header-left">
                    <div className="logo">
                        <span className="logo-text">TaskFlow</span>
                    </div>
                    <nav className="header-nav">
                        <button
                            className={`nav-button ${activeTab === 'board' ? 'active' : ''}`}
                            onClick={() => onTabChange('board')}
                        >
                            Доска
                        </button>
                        <button
                            className={`nav-button ${activeTab === 'analytics' ? 'active' : ''}`}
                            onClick={() => onTabChange('analytics')}
                        >
                            <FaChartLine className="nav-icon"/>
                            <span>Аналитика</span>
                        </button>
                        <button
                            className={`nav-button ${activeTab === 'docs' ? 'active' : ''}`}
                            onClick={() => onTabChange('docs')}
                        >
                            <FaFileAlt className="nav-icon"/>
                            <span>Документация</span>
                        </button>
                    </nav>
                </div>

                {/* Правая часть хедера с аутентификацией */}
                <div className="header-right">
                    {isAuthenticated ? (
                        <div className="auth-user-section">
                            <div className="user-info">
                                <FaUserCircle className="user-icon"/>
                                <span className="user-name">
                  {userFullName || 'Пользователь'}
                </span>
                            </div>
                            <button
                                className="auth-button logout-button"
                                onClick={onLogout}
                                title="Выйти из системы"
                            >
                                <FaSignOutAlt className="auth-icon"/>
                                <span className="auth-text">Выйти</span>
                            </button>
                        </div>
                    ) : (
                        <button
                            className="auth-button login-button"
                            onClick={onLogin}
                            title="Войти в систему"
                        >
                            <FaSignInAlt className="auth-icon"/>
                            <span className="auth-text">Войти</span>
                        </button>
                    )}
                </div>
            </div>
        </header>
    );
}

export default Header;


