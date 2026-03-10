import { useEffect, useState } from 'react'
import { getShopItems } from '../services/shopService'
import './Shop.css'

function Shop() {
  const [items, setItems] = useState([])

  useEffect(() => {
    let isMounted = true
    const loadShopItems = async () => {
      try {
        const data = await getShopItems()
        const itemData = Array.isArray(data) ? data : data?.items || []
        if (isMounted) {
          setItems(itemData)
        }
      } catch (error) {
        console.error('Failed to load shop items', error)
      }
    }
    loadShopItems()
    return () => {
      isMounted = false
    }
  }, [])

  const handlePurchase = (item) => {
    alert(`Đã mua: ${item.name}\n-${item.price} lingots`)
  }

  return (
    <div className="shop-page">
      <div className="shop-header">
        <h1>Cửa hàng</h1>
        <p>Mua vật phẩm hữu ích cho hành trình học tập</p>
      </div>

      {/* Currency Balance */}
      <div className="currency-balance">
        <div className="balance-card">
          <span className="currency-icon">💎</span>
          <div className="balance-info">
            <h3>Lingots của bạn</h3>
            <p className="balance-amount">1,250</p>
          </div>
        </div>
        <div className="earn-info">
          <p>💡 Kiếm Lingots bằng cách hoàn thành bài học và thử thách!</p>
        </div>
      </div>

      {/* Shop Categories */}
      <div className="shop-categories">
        <button className="category-btn active">Tất cả</button>
        <button className="category-btn">Power-ups</button>
        <button className="category-btn">Trang phục</button>
      </div>

      {/* Shop Items */}
      <div className="shop-grid">
        {items.map((item) => (
          <div key={item.id} className="shop-item">
            <div className="item-icon">{item.icon}</div>
            <div className="item-content">
              <h3>{item.name}</h3>
              <p>{item.description}</p>
              <div className="item-footer">
                <div className="item-price">
                  <span className="price-icon">💎</span>
                  <span className="price-amount">{item.price}</span>
                </div>
                <button 
                  className="buy-btn"
                  onClick={() => handlePurchase(item)}
                >
                  Mua
                </button>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Special Offers */}
      <div className="special-offers">
        <h2>Ưu đãi đặc biệt</h2>
        <div className="offer-card">
          <div className="offer-badge">🎁 Giảm 50%</div>
          <div className="offer-content">
            <h3>Gói Super Duolingo</h3>
            <p>Truy cập không giới hạn tất cả tính năng cao cấp</p>
            <ul className="offer-features">
              <li>✓ Không quảng cáo</li>
              <li>✓ Trái tim không giới hạn</li>
              <li>✓ Bài tập được cá nhân hóa</li>
              <li>✓ Luyện tập lỗi sai miễn phí</li>
            </ul>
            <button className="offer-btn">Dùng thử 2 tuần miễn phí</button>
          </div>
        </div>
      </div>

      {/* Tips */}
      <div className="shop-tips">
        <h3>💰 Cách kiếm Lingots</h3>
        <div className="tips-grid">
          <div className="tip-card">
            <span className="tip-icon">📚</span>
            <h4>Hoàn thành bài học</h4>
            <p>+10 lingots mỗi bài</p>
          </div>
          <div className="tip-card">
            <span className="tip-icon">🏆</span>
            <h4>Đạt thành tích</h4>
            <p>+50 lingots mỗi thành tích</p>
          </div>
          <div className="tip-card">
            <span className="tip-icon">🎯</span>
            <h4>Thử thách hàng ngày</h4>
            <p>+20 lingots mỗi ngày</p>
          </div>
          <div className="tip-card">
            <span className="tip-icon">👥</span>
            <h4>Mời bạn bè</h4>
            <p>+100 lingots mỗi bạn</p>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Shop
