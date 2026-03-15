import {HeaderProps} from '../types';
import {FaChartLine, FaFileAlt} from 'react-icons/fa';
import { AuthenticatedComponent } from './AuthenticatedComponent';
import './HeaderKanbanBoard.css';

function Header({activeTab, onTabChange, authInfo}: HeaderProps) {

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

                <AuthenticatedComponent authInfo={authInfo} />
            </div>
        </header>
    );
}

export default Header;


