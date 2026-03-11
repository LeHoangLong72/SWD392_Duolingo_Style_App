import { useEffect, useState } from 'react'
import './App.css'
import MainLayout from './layouts/MainLayout'
import Learning from './pages/Learning'
import Practice from './pages/Practice'
import Leaderboard from './pages/Leaderboard'
import Shop from './pages/Shop'
import DailyStreak from './pages/DailyStreak'
import Auth from './pages/Auth'

function App() {
  const [user, setUser] = useState(null)
  const [currentTab, setCurrentTab] = useState('learning')
  const [streak, setStreak] = useState(7)
  const [xp, setXp] = useState(1250)
  const [showAuth, setShowAuth] = useState(false)
  const [authMode, setAuthMode] = useState('login') // 'login' or 'signup'
  const [showLogoutConfirm, setShowLogoutConfirm] = useState(false)

  useEffect(() => {
    const token = localStorage.getItem('token')
    const storedUser = localStorage.getItem('user')
    if (token && storedUser) {
      try {
        const parsedUser = JSON.parse(storedUser)
        setUser(parsedUser)
      } catch (error) {
        console.error('Failed to parse stored user', error)
      }
    }
  }, [])

  const handleLogin = (userData) => {
    setUser(userData)
    setShowAuth(false)
    setCurrentTab('learning')
  }

  const handleLogout = () => {
    setUser(null)
    setShowLogoutConfirm(false)
    localStorage.removeItem('token')
    localStorage.removeItem('user')
  }
  const requestLogout = () => {
    setShowLogoutConfirm(true)
  }

  const cancelLogout = () => {
    setShowLogoutConfirm(false)
  }

  const openAuth = (mode) => {
    setAuthMode(mode)
    setShowAuth(true)
  }

  const closeAuth = () => {
    setShowAuth(false)
  }

  const renderContent = () => {
    switch(currentTab) {
      case 'learning':
        return <Learning />
      case 'practice':
        return <Practice />
      case 'leaderboard':
        return <Leaderboard />
      case 'shop':
        return <Shop />
      case 'streak':
        return <DailyStreak />
      default:
        return <Learning />
    }
  }

  return (
    <div className="app">
      {/* Using MainLayout template for consistent page structure */}
      <MainLayout
        user={user}
        streak={streak}
        xp={xp}
        currentTab={currentTab}
        onTabChange={setCurrentTab}
        onLogoutRequest={requestLogout}
        onOpenAuth={openAuth}
      >
        {renderContent()}
      </MainLayout>

      {/* Auth Modal */}
      {showAuth && (
        <Auth 
          onLogin={handleLogin} 
          onClose={closeAuth}
          initialMode={authMode}
        />
      )}

      {/* Logout Confirm */}
      {showLogoutConfirm && (
        <div className="logout-modal-overlay" onClick={cancelLogout}>
          <div className="logout-modal" onClick={(e) => e.stopPropagation()}>
            <h3>Bạn có muốn đăng xuất không?</h3>
            <div className="logout-actions">
              <button className="logout-confirm-btn" onClick={handleLogout} type="button">
                Có
              </button>
              <button className="logout-cancel-btn" onClick={cancelLogout} type="button">
                Không
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default App
