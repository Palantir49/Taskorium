import { AuthenticatedComponent } from './AuthenticatedComponent';
import { AuthInfo } from '../types';
import './HeaderStartBoard.css';

interface HeaderStartBoardProps {
  authInfo: AuthInfo;
}

export const HeaderStartBoard = ({ authInfo }: HeaderStartBoardProps) => {
  return (
    <header className="header-start">
      <div className="header-content">
        <div className="logo">

        </div>
        <AuthenticatedComponent authInfo={authInfo} />
      </div>
    </header>
  );
};