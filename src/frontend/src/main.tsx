import React from 'react'
import ReactDOM from 'react-dom/client'
import App from "./App";
import './index.css'
import {AuthProvider} from "react-oidc-context";
import {WebStorageStateStore} from 'oidc-client-ts'

const oidcConfig = {
    authority: import.meta.env.VITE_OIDC_AUTHORITY,
    client_id: import.meta.env.VITE_APP_OIDC_CLIENT_ID,
    redirect_uri: window.location.origin,
    post_logout_redirect_uri: window.location.origin,
    response_type: "code",
    automaticSilentRenew: true,
    loadUserInfo: true,
    userStore: new WebStorageStateStore({store: window.localStorage}),
    onSigninCallback: () => {
        const url = new URL(window.location.href);
        url.searchParams.delete('code');
        url.searchParams.delete('state');
        url.searchParams.delete('session_state');
        url.searchParams.delete('iss');
        window.history.replaceState({}, '', url.toString());
    },
};


const container = document.getElementById('root');

if (!container) {
    throw new Error("Root container with id 'root' not found");
}

ReactDOM.createRoot(container).render(
    <React.StrictMode>
        <AuthProvider {...oidcConfig}>
            <App/>
        </AuthProvider>
    </React.StrictMode>
)