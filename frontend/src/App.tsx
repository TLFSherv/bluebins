import { Routes, Route } from 'react-router-dom'
import Home from './Pages/Home'
import SignUp from './Pages/SignUp'
import SignIn from './Pages/SignIn'
import Error from './Pages/Error'
import Layout from './Layout'

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/sign-up" element={<SignUp />} />
        <Route path="/sign-in" element={<SignIn />} />
        <Route path="/error" element={<Error />} />
      </Routes>
    </Layout>
  )
}

export default App