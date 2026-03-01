import { FaSignInAlt, FaSignOutAlt, FaUserCircle } from 'react-icons/fa';
import { AuthInfo } from '../types';

interface HeaderAuthorizationProps {
  authInfo: AuthInfo;
}

export const HeaderAuthorization = ({ authInfo }: HeaderAuthorizationProps) => {
  const { isAuthenticated, userFullName, onLogin, onLogout } = authInfo || {};

  return (
    <div className="header-right">
      {isAuthenticated ? (
        <div className="auth-user-section">
          <div className="user-info">
            <FaUserCircle className="user-icon" />
            <span className="user-name">
              {userFullName || 'Пользователь'}
            </span>
          </div>
          <button
            className="auth-button logout-button"
            onClick={onLogout}
            title="Выйти из системы"
          >
            <FaSignOutAlt className="auth-icon" />
            <span className="auth-text">Выйти</span>
          </button>
        </div>
      ) : (
        <button
          className="auth-button login-button"
          onClick={onLogin}
          title="Войти в систему"
        >
          <FaSignInAlt className="auth-icon" />
          <span className="auth-text">Войти</span>
        </button>
      )}
    </div>
  );
};