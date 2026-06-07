// components/ErrorBoundary.tsx
import { useRouteError, isRouteErrorResponse } from 'react-router';

export const ErrorBoundary = () => {
    const error = useRouteError();
    console.error(error);

    if (isRouteErrorResponse(error)) {
        return (
            <div style={{ padding: '20px', color: 'red' }}>
                <h1>Oops! {error.status}</h1>
                <p>{error.statusText || 'Page not found.'}</p>
            </div>
        );
    }

    return (
        <div style={{ padding: '20px', color: 'red' }}>
            <h1>Something went wrong!</h1>
            <p>{error instanceof Error ? error.message : 'An unexpected error occurred.'}</p>
        </div>
    );
};