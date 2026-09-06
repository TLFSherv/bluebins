import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider
} from 'react-router';
import Home from './index';
import SignIn from './features/account/pages/SignIn'
import SignUp from './features/account/pages/SignUp'
import Layout from './components/Layout'
import Dashboard from './features/dashboard'
import Booking from './features/bookings'
import { ErrorBoundary } from './components/ErrorBoundary';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<Layout />} errorElement={<ErrorBoundary />}>
      <Route index element={<Home />} />
      <Route path='account'>
        <Route path='sign-in' element={<SignIn />} />
        <Route path='sign-up' element={<SignUp />} />
      </Route>
      <Route path='/'>
        <Route path='dashboard' element={<Dashboard />} />
        <Route path='booking' element={<Booking />} />
      </Route>
    </Route >
  )
)
function App() {
  return <RouterProvider router={router} />
}

export default App