import {
  createBrowserRouter,
  createRoutesFromElements,
  Route,
  RouterProvider
} from 'react-router';
import Home from './Home'
import SignIn from './Recyclers/Pages/Auth/SignIn'
import SignUp from './Recyclers/Pages/Auth/SignUp'
import Layout from './Components/Layout'
import Dashboard from './Recyclers/Pages/Portal/Dashboard'
import Booking from './Recyclers/Pages/Portal/Booking'
import { ErrorBoundary } from './Components/ErrorBoundary';

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<Layout />} errorElement={<ErrorBoundary />}>
      <Route index element={<Home />} />
      <Route path='auth'>
        <Route path='sign-in' element={<SignIn />} />
        <Route path='sign-up' element={<SignUp />} />
      </Route>
      <Route path='portal'>
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