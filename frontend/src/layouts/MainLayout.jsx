/**
 * MainLayout - Shared template for all authenticated pages
 * Provides consistent header, navigation, and content wrapper
 * Used by: Learning, Practice, Shop, Leaderboard, DailyStreak pages
 */

import './MainLayout.css'

function MainLayout({ 
  children, 
  user, 
  streak, 
  xp, 
  currentTab, 
  onTabChange,
  onLogoutRequest,
  onOpenAuth 
}) {
  return (
    <>
      {/* Header */}
      <header className="header">
        <div className="header-left">
          <div className="logo">
            <span className="logo-icon">🦉</span>
            <span className="logo-text">Học tiếng Nhật</span>
          </div>
        </div>
        <div className="header-center">
          <nav className="nav">
            <button 
              className={`nav-btn ${currentTab === 'learning' ? 'active' : ''}`}
              onClick={() => onTabChange('learning')}
            >
              Học tập
            </button>
            <button 
              className={`nav-btn ${currentTab === 'practice' ? 'active' : ''}`}
              onClick={() => onTabChange('practice')}
            >
              Thực hành
            </button>
            <button 
              className={`nav-btn ${currentTab === 'leaderboard' ? 'active' : ''}`}
              onClick={() => onTabChange('leaderboard')}
            >
              Bảng xếp hạng
            </button>
            <button 
              className={`nav-btn ${currentTab === 'shop' ? 'active' : ''}`}
              onClick={() => onTabChange('shop')}
            >
              Cửa hàng
            </button>
          </nav>
        </div>
        <div className="header-right">
          <div 
            className={`streak ${currentTab === 'streak' ? 'active' : ''}`}
            onClick={() => onTabChange('streak')}
            style={{ cursor: 'pointer' }}
          >
            <span className="streak-icon">🔥</span>
            <span className="streak-count">{streak}</span>
          </div>
          <div className="xp">
            <span className="xp-icon">⭐</span>
            <span className="xp-count">{xp}</span>
          </div>
          {user ? (
            <>
              <div className="profile">
                <div className="profile-avatar">👤</div>
                <span className="profile-name">{user.name}</span>
              </div>
              <button
                className="logout-btn"
                onClick={onLogoutRequest}
                type="button"
                title="Đăng xuất"
              >
                ⎋
              </button>
            </>
          ) : (
            <div className="auth-buttons">
              <button className="auth-header-btn login-btn" onClick={() => onOpenAuth('login')}>
                ĐĂNG NHẬP
              </button>
              <button className="auth-header-btn signup-btn" onClick={() => onOpenAuth('signup')}>
                ĐĂNG KÝ
              </button>
            </div>
          )}
        </div>
      </header>

      {/* Main Content */}
      <main className="main-content">
        {children}
      </main>
    </>
  )
}

export default MainLayout
