import 'package:flutter/material.dart';

class CharactersScreen extends StatelessWidget {
  const CharactersScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F7F7),
      body: CustomScrollView(
        slivers: [
          // Top Header
          SliverToBoxAdapter(
            child: Container(
              color: Colors.white,
              padding: const EdgeInsets.all(20),
              child: Column(
                children: [
                  const SizedBox(height: 20),
                  // Stats Row
                  Row(
                    mainAxisAlignment: MainAxisAlignment.end,
                    children: [
                      _buildHeaderStat('�🇵', '1'),
                      const SizedBox(width: 12),
                      _buildHeaderStat('🔥', '0'),
                      const SizedBox(width: 12),
                      _buildHeaderStat('💎', '500'),
                      const SizedBox(width: 12),
                      _buildHeaderStat('❤️', '5'),
                    ],
                  ),
                  const SizedBox(height: 30),
                  // Title
                  const Text(
                    'Học bảng chữ cái tiếng Nhật',
                    style: TextStyle(
                      fontSize: 28,
                      fontWeight: FontWeight.bold,
                      color: Colors.black87,
                    ),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 12),
                  Text(
                    'Học cách đọc và viết Hiragana & Katakana',
                    style: TextStyle(
                      fontSize: 16,
                      color: Colors.grey[600],
                    ),
                    textAlign: TextAlign.center,
                  ),
                  const SizedBox(height: 24),
                  // Start Button
                  Container(
                    width: double.infinity,
                    constraints: const BoxConstraints(maxWidth: 500),
                    child: ElevatedButton(
                      onPressed: () {},
                      style: ElevatedButton.styleFrom(
                        backgroundColor: const Color(0xFF1CB0F6),
                        foregroundColor: Colors.white,
                        padding: const EdgeInsets.symmetric(vertical: 16),
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(16),
                        ),
                        elevation: 0,
                      ),
                      child: const Text(
                        'BẮT ĐẦU +10 XP',
                        style: TextStyle(
                          fontSize: 16,
                          fontWeight: FontWeight.bold,
                          letterSpacing: 0.5,
                        ),
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
          // Hiragana Section
          SliverToBoxAdapter(
            child: Padding(
              padding: const EdgeInsets.all(20),
              child: Column(
                children: [
                  const Divider(height: 40),
                  const Text(
                    'Hiragana Cơ Bản (あ-の)',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.black87,
                    ),
                  ),
                  const SizedBox(height: 20),
                ],
              ),
            ),
          ),
          // Hiragana Grid
          SliverPadding(
            padding: const EdgeInsets.symmetric(horizontal: 20),
            sliver: SliverGrid(
              gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
                crossAxisCount: 3,
                childAspectRatio: 1.3,
                crossAxisSpacing: 12,
                mainAxisSpacing: 12,
              ),
              delegate: SliverChildListDelegate([
                _buildCharacterCard('あ', 'a', 0.3),
                _buildCharacterCard('い', 'i', 0.3),
                _buildCharacterCard('う', 'u', 0.3),
                _buildCharacterCard('え', 'e', 0.2),
                _buildCharacterCard('お', 'o', 0.2),
                _buildCharacterCard('か', 'ka', 0.0),
                _buildCharacterCard('き', 'ki', 0.0),
                _buildCharacterCard('く', 'ku', 0.0),
                _buildCharacterCard('け', 'ke', 0.0),
                _buildCharacterCard('こ', 'ko', 0.0),
                _buildCharacterCard('さ', 'sa', 0.0),
                _buildCharacterCard('し', 'shi', 0.0),
                _buildCharacterCard('す', 'su', 0.0),
                _buildCharacterCard('せ', 'se', 0.0),
                _buildCharacterCard('そ', 'so', 0.0),
                _buildCharacterCard('た', 'ta', 0.0),
                _buildCharacterCard('ち', 'chi', 0.0),
                _buildCharacterCard('つ', 'tsu', 0.0),
                _buildCharacterCard('て', 'te', 0.0),
                _buildCharacterCard('と', 'to', 0.0),
                _buildCharacterCard('な', 'na', 0.0),
                _buildCharacterCard('に', 'ni', 0.0),
                _buildCharacterCard('ぬ', 'nu', 0.0),
                _buildCharacterCard('ね', 'ne', 0.0),
                _buildCharacterCard('の', 'no', 0.0),
              ]),
            ),
          ),
          const SliverToBoxAdapter(child: SizedBox(height: 40)),
        ],
      ),
    );
  }

  Widget _buildHeaderStat(String emoji, String value) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 6),
      decoration: BoxDecoration(
        color: Colors.grey[100],
        borderRadius: BorderRadius.circular(20),
      ),
      child: Row(
        children: [
          Text(emoji, style: const TextStyle(fontSize: 16)),
          const SizedBox(width: 6),
          Text(
            value,
            style: const TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 15,
              color: Colors.black87,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildCharacterCard(String phonetic, String example, double progress) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: Colors.grey[300]!, width: 2),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.04),
            blurRadius: 8,
            offset: const Offset(0, 2),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          onTap: () {},
          borderRadius: BorderRadius.circular(16),
          child: Padding(
            padding: const EdgeInsets.all(12),
            child: Column(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Text(
                  phonetic,
                  style: const TextStyle(
                    fontSize: 32,
                    fontWeight: FontWeight.bold,
                    color: Colors.black87,
                  ),
                ),
                const SizedBox(height: 4),
                Text(
                  example,
                  style: TextStyle(
                    fontSize: 14,
                    color: Colors.grey[600],
                  ),
                ),
                if (progress > 0) ...[
                  const SizedBox(height: 8),
                  ClipRRect(
                    borderRadius: BorderRadius.circular(4),
                    child: LinearProgressIndicator(
                      value: progress,
                      backgroundColor: Colors.grey[200],
                      valueColor: const AlwaysStoppedAnimation<Color>(
                        Color(0xFFFFD700),
                      ),
                      minHeight: 4,
                    ),
                  ),
                ],
              ],
            ),
          ),
        ),
      ),
    );
  }
}
