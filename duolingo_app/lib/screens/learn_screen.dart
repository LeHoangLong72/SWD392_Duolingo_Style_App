import 'package:flutter/material.dart';

class LearnScreen extends StatelessWidget {
  const LearnScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFFF7F7F7),
      body: CustomScrollView(
        slivers: [
          // Header with Unit Title and Stats
          SliverAppBar(
            expandedHeight: 180,
            floating: false,
            pinned: true,
            backgroundColor: Colors.white,
            elevation: 0,
            flexibleSpace: FlexibleSpaceBar(
              background: Container(
                decoration: const BoxDecoration(
                  gradient: LinearGradient(
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                    colors: [Color(0xFF58CC02), Color(0xFF89E24D)],
                  ),
                ),
                child: SafeArea(
                  child: Padding(
                    padding: const EdgeInsets.symmetric(horizontal: 20.0, vertical: 12),
                    child: Column(
                      children: [
                        // Top Stats Row
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            // Back arrow
                            IconButton(
                              icon: const Icon(Icons.arrow_back, color: Colors.white),
                              onPressed: () {},
                            ),
                            // Stats
                            Row(
                              children: [
                                _buildHeaderStat('�🇵', '1'),
                                const SizedBox(width: 8),
                                _buildHeaderStat('🔥', '0'),
                                const SizedBox(width: 8),
                                _buildHeaderStat('💎', '500'),
                                const SizedBox(width: 8),
                                _buildHeaderStat('❤️', '5'),
                              ],
                            ),
                          ],
                        ),
                        const Spacer(),
                        // Unit Title
                        Row(
                          mainAxisAlignment: MainAxisAlignment.spaceBetween,
                          children: [
                            const Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'PHẦN 1, CỦA 10',
                                    style: TextStyle(
                                      color: Colors.white,
                                      fontSize: 13,
                                      fontWeight: FontWeight.bold,
                                      letterSpacing: 0.5,
                                    ),
                                  ),
                                  SizedBox(height: 4),
                                  Text(
                                    'Chào hỏi cơ bản',
                                    style: TextStyle(
                                      color: Colors.white,
                                      fontSize: 22,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            Container(
                              padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 10),
                              decoration: BoxDecoration(
                                color: Colors.white,
                                borderRadius: BorderRadius.circular(12),
                              ),
                              child: Row(
                                children: [
                                  const Icon(
                                    Icons.menu_book,
                                    color: Color(0xFF58CC02),
                                    size: 20,
                                  ),
                                  const SizedBox(width: 8),
                                  const Text(
                                    'HƯỚNG DẪN',
                                    style: TextStyle(
                                      fontSize: 13,
                                      fontWeight: FontWeight.bold,
                                      color: Color(0xFF58CC02),
                                      letterSpacing: 0.3,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 12),
                      ],
                    ),
                  ),
                ),
              ),
            ),
          ),
          SliverToBoxAdapter(
            child: Column(
              children: [
                const SizedBox(height: 20),
                _buildUnitHeader(
                    'Unit 1', 'Bảng chữ cái Nhật', const Color(0xFF58CC02)),
                const SizedBox(height: 16),
                // Guidebook button
                Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 20.0),
                  child: Container(
                    width: double.infinity,
                    padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 12),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: const Color(0xFF58CC02), width: 2),
                    ),
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: [
                        Icon(Icons.menu_book, color: const Color(0xFF58CC02), size: 20),
                        const SizedBox(width: 8),
                        const Text(
                          'Hướng dẫn Unit',
                          style: TextStyle(
                            fontSize: 15,
                            fontWeight: FontWeight.bold,
                            color: Color(0xFF58CC02),
                          ),
                        ),
                      ],
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                _buildLearningPath(context),
                const SizedBox(height: 32),
                // Practice Hub section
                Padding(
                  padding: const EdgeInsets.symmetric(horizontal: 20.0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Text(
                        'Trung tâm luyện tập',
                        style: TextStyle(
                          fontSize: 20,
                          fontWeight: FontWeight.bold,
                          color: Colors.black87,
                        ),
                      ),
                      const SizedBox(height: 12),
                      Row(
                        children: [
                          Expanded(
                            child: _buildPracticeCard(
                              '💪',
                              'Luyện tập',
                              'Không giới hạn trái tim',
                              const Color(0xFFFF9800),
                            ),
                          ),
                          const SizedBox(width: 12),
                          Expanded(
                            child: _buildPracticeCard(
                              '📖',
                              'Câu chuyện',
                              'Mới để mở khóa',
                              const Color(0xFF9C27B0),
                            ),
                          ),
                        ],
                      ),
                    ],
                  ),
                ),
                const SizedBox(height: 100),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildHeaderStat(String emoji, String value) {
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(
        color: Colors.white.withOpacity(0.25),
        borderRadius: BorderRadius.circular(16),
      ),
      child: Row(
        children: [
          Text(emoji, style: const TextStyle(fontSize: 16)),
          const SizedBox(width: 4),
          Text(
            value,
            style: const TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 14,
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildUnitHeader(String unitNumber, String title, Color color) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 20.0),
      child: Container(
        width: double.infinity,
        padding: const EdgeInsets.all(16),
        decoration: BoxDecoration(
          color: Colors.white,
          borderRadius: BorderRadius.circular(16),
          boxShadow: [
            BoxShadow(
              color: Colors.black.withOpacity(0.06),
              blurRadius: 8,
              offset: const Offset(0, 2),
            ),
          ],
        ),
        child: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(12),
              decoration: BoxDecoration(
                color: color.withOpacity(0.15),
                borderRadius: BorderRadius.circular(12),
              ),
              child: Icon(Icons.book_outlined, color: color, size: 28),
            ),
            const SizedBox(width: 16),
            Expanded(
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    unitNumber,
                    style: TextStyle(
                      fontSize: 13,
                      fontWeight: FontWeight.bold,
                      color: color,
                      letterSpacing: 0.5,
                    ),
                  ),
                  const SizedBox(height: 2),
                  Text(
                    title,
                    style: const TextStyle(
                      fontSize: 18,
                      fontWeight: FontWeight.bold,
                      color: Colors.black87,
                    ),
                  ),
                  const SizedBox(height: 4),
                  Text(
                    'Học cách viết và phát âm',
                    style: TextStyle(
                      fontSize: 13,
                      color: Colors.grey[600],
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildLearningPath(BuildContext context) {
    final screenWidth = MediaQuery.of(context).size.width;

    return Column(
      children: [
        // Level 1
        _buildPathNode(
          'Hiragana あ-そ',
          'Bài học 1',
          0.7,
          const Color(0xFF58CC02),
          position: screenWidth * 0.30,
          status: LessonStatus.inProgress,
          icon: '🌸',
        ),
        _buildPathConnector(screenWidth * 0.30),
        
        // Level 2
        _buildPathNode(
          'Hiragana た-ほ',
          'Bài học 2',
          0.4,
          const Color(0xFF58CC02),
          position: screenWidth * 0.55,
          status: LessonStatus.available,
          icon: '🎌',
        ),
        _buildPathConnector(screenWidth * 0.55),
        
        // Jump Here Button
        Padding(
          padding: EdgeInsets.only(left: screenWidth * 0.55 + 10),
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 6),
            decoration: BoxDecoration(
              color: const Color(0xFF1CB0F6),
              borderRadius: BorderRadius.circular(16),
              boxShadow: [
                BoxShadow(
                  color: const Color(0xFF1CB0F6).withOpacity(0.3),
                  blurRadius: 8,
                  offset: const Offset(0, 4),
                ),
              ],
            ),
            child: const Row(
              mainAxisSize: MainAxisSize.min,
              children: [
                Icon(Icons.arrow_upward, color: Colors.white, size: 14),
                SizedBox(width: 4),
                Text(
                  'NHẢY ĐẾN ĐÂY',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 11,
                    fontWeight: FontWeight.bold,
                    letterSpacing: 0.5,
                  ),
                ),
              ],
            ),
          ),
        ),
        _buildPathConnector(screenWidth * 0.55),
        
        // Level 3
        _buildPathNode(
          'Hiragana ま-ん',
          'Bài học 3',
          0.0,
          const Color(0xFF58CC02),
          position: screenWidth * 0.15,
          status: LessonStatus.available,
          icon: '✏️',
        ),
        _buildPathConnector(screenWidth * 0.15),
        
        // Review Node
        _buildReviewNode(screenWidth * 0.40),
        _buildPathConnector(screenWidth * 0.40),
        
        // Unit 2 Header
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
          child: _buildUnitHeader(
              'Unit 2', 'Bảng chữ cái Katakana', const Color(0xFF1CB0F6)),
        ),
        
        // Level 4
        _buildPathNode(
          'Katakana ア-ソ',
          'Bài học 4',
          0.0,
          const Color(0xFF1CB0F6),
          position: screenWidth * 0.45,
          status: LessonStatus.locked,
          icon: '📝',
        ),
        _buildPathConnector(screenWidth * 0.45),
        
        // Level 5
        _buildPathNode(
          'Katakana タ-ホ',
          'Bài học 5',
          0.0,
          const Color(0xFF1CB0F6),
          position: screenWidth * 0.20,
          status: LessonStatus.locked,
          icon: '📚',
        ),
        _buildPathConnector(screenWidth * 0.20),
        
        // Chest
        _buildChestNode(screenWidth * 0.50, locked: true),
        _buildPathConnector(screenWidth * 0.50),
        
        // Unit 3 Header
        Padding(
          padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 20),
          child: _buildUnitHeader(
              'Unit 3', 'Giao tiếp cơ bản', const Color(0xFFFF9800)),
        ),
        
        // Level 6
        _buildPathNode(
          'Chào hỏi',
          'Bài học 6',
          0.0,
          const Color(0xFFFF9800),
          position: screenWidth * 0.30,
          status: LessonStatus.locked,
          icon: '👋',
        ),
      ],
    );
  }

  Widget _buildPathNode(
    String title,
    String subtitle,
    double progress,
    Color color, {
    required double position,
    required LessonStatus status,
    required String icon,
  }) {
    return Padding(
      padding: EdgeInsets.only(left: position),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.center,
        children: [
          GestureDetector(
            onTap: status != LessonStatus.locked ? () {} : null,
            child: Stack(
              clipBehavior: Clip.none,
              children: [
                Container(
                  width: 85,
                  height: 85,
                  decoration: BoxDecoration(
                    color: status == LessonStatus.locked
                        ? Colors.grey[300]
                        : color,
                    shape: BoxShape.circle,
                    border: Border.all(
                      color: Colors.white,
                      width: 5,
                    ),
                    boxShadow: [
                      BoxShadow(
                        color: (status == LessonStatus.locked
                                ? Colors.grey
                                : color)
                            .withOpacity(0.4),
                        blurRadius: 12,
                        offset: const Offset(0, 6),
                      ),
                    ],
                  ),
                  child: Center(
                    child: status == LessonStatus.locked
                        ? const Icon(
                            Icons.lock_outline,
                            color: Colors.white,
                            size: 38,
                          )
                        : status == LessonStatus.completed
                            ? const Icon(
                                Icons.check_circle,
                                color: Colors.white,
                                size: 38,
                              )
                            : Text(
                                icon,
                                style: const TextStyle(fontSize: 36),
                              ),
                  ),
                ),
                if (progress > 0 &&
                    status != LessonStatus.completed &&
                    status != LessonStatus.locked)
                  Positioned(
                    bottom: -8,
                    left: 10,
                    right: 10,
                    child: Container(
                      height: 10,
                      decoration: BoxDecoration(
                        color: Colors.white,
                        borderRadius: BorderRadius.circular(5),
                        border: Border.all(color: Colors.grey[300]!, width: 2),
                      ),
                      child: FractionallySizedBox(
                        alignment: Alignment.centerLeft,
                        widthFactor: progress,
                        child: Container(
                          decoration: BoxDecoration(
                            color: const Color(0xFFFFD700),
                            borderRadius: BorderRadius.circular(5),
                          ),
                        ),
                      ),
                    ),
                  ),
                if (status == LessonStatus.completed)
                  Positioned(
                    top: -5,
                    right: -5,
                    child: Container(
                      padding: const EdgeInsets.all(4),
                      decoration: const BoxDecoration(
                        color: Color(0xFFFFD700),
                        shape: BoxShape.circle,
                      ),
                      child: const Icon(
                        Icons.star,
                        color: Colors.white,
                        size: 16,
                      ),
                    ),
                  ),
                if (status == LessonStatus.inProgress)
                  Positioned(
                    bottom: 0,
                    left: 0,
                    right: 0,
                    child: Row(
                      mainAxisAlignment: MainAxisAlignment.center,
                      children: List.generate(
                        3,
                        (index) => Padding(
                          padding: const EdgeInsets.symmetric(horizontal: 2),
                          child: Icon(
                            index < 1 ? Icons.star : Icons.star_border,
                            color: const Color(0xFFFFD700),
                            size: 16,
                          ),
                        ),
                      ),
                    ),
                  ),
              ],
            ),
          ),
          const SizedBox(height: 10),
          Container(
            width: 110,
            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 6),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(10),
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(0.08),
                  blurRadius: 6,
                  offset: const Offset(0, 2),
                ),
              ],
            ),
            child: Column(
              children: [
                Text(
                  subtitle,
                  style: TextStyle(
                    fontSize: 10,
                    color: Colors.grey[600],
                    fontWeight: FontWeight.w600,
                  ),
                ),
                const SizedBox(height: 2),
                Text(
                  title,
                  style: const TextStyle(
                    fontSize: 12,
                    fontWeight: FontWeight.bold,
                    color: Colors.black87,
                  ),
                  maxLines: 2,
                  overflow: TextOverflow.ellipsis,
                  textAlign: TextAlign.center,
                ),
                if (status != LessonStatus.locked && status != LessonStatus.completed) ...[
                  const SizedBox(height: 4),
                  Container(
                    padding: const EdgeInsets.symmetric(horizontal: 6, vertical: 2),
                    decoration: BoxDecoration(
                      color: const Color(0xFFFFD700).withOpacity(0.2),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Text(
                      '+10 XP',
                      style: TextStyle(
                        fontSize: 10,
                        fontWeight: FontWeight.bold,
                        color: Color(0xFFFF9800),
                      ),
                    ),
                  ),
                ],
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPathConnector(double position) {
    return Padding(
      padding: EdgeInsets.only(left: position + 40),
      child: Container(
        height: 35,
        width: 5,
        decoration: BoxDecoration(
          color: Colors.grey[300],
          borderRadius: BorderRadius.circular(3),
        ),
      ),
    );
  }

  Widget _buildReviewNode(double position) {
    return Padding(
      padding: EdgeInsets.only(left: position),
      child: Column(
        children: [
          Container(
            width: 85,
            height: 85,
            decoration: BoxDecoration(
              gradient: const LinearGradient(
                begin: Alignment.topLeft,
                end: Alignment.bottomRight,
                colors: [Color(0xFFFFD700), Color(0xFFFFA500)],
              ),
              shape: BoxShape.circle,
              border: Border.all(color: Colors.white, width: 5),
              boxShadow: [
                BoxShadow(
                  color: const Color(0xFFFFD700).withOpacity(0.4),
                  blurRadius: 12,
                  offset: const Offset(0, 6),
                ),
              ],
            ),
            child: const Center(
              child: Text(
                '🏆',
                style: TextStyle(fontSize: 38),
              ),
            ),
          ),
          const SizedBox(height: 10),
          Container(
            width: 110,
            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 6),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(10),
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(0.08),
                  blurRadius: 6,
                  offset: const Offset(0, 2),
                ),
              ],
            ),
            child: const Column(
              children: [
                Text(
                  'ÔN TẬP',
                  style: TextStyle(
                    fontSize: 11,
                    fontWeight: FontWeight.bold,
                    color: Colors.black87,
                  ),
                ),
                Text(
                  'Unit 1',
                  style: TextStyle(
                    fontSize: 10,
                    color: Colors.grey,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildChestNode(double position, {bool locked = false}) {
    return Padding(
      padding: EdgeInsets.only(left: position),
      child: Column(
        children: [
          Container(
            width: 95,
            height: 95,
            decoration: BoxDecoration(
              gradient: locked
                  ? LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: [Colors.grey[400]!, Colors.grey[600]!],
                    )
                  : const LinearGradient(
                      begin: Alignment.topLeft,
                      end: Alignment.bottomRight,
                      colors: [Color(0xFF9C27B0), Color(0xFFE91E63)],
                    ),
              borderRadius: BorderRadius.circular(20),
              border: Border.all(color: Colors.white, width: 5),
              boxShadow: [
                BoxShadow(
                  color: (locked ? Colors.grey : const Color(0xFF9C27B0))
                      .withOpacity(0.4),
                  blurRadius: 12,
                  offset: const Offset(0, 6),
                ),
              ],
            ),
            child: Center(
              child: Text(
                locked ? '🔒' : '🎁',
                style: const TextStyle(fontSize: 42),
              ),
            ),
          ),
          const SizedBox(height: 10),
          Container(
            width: 110,
            padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 6),
            decoration: BoxDecoration(
              color: Colors.white,
              borderRadius: BorderRadius.circular(10),
              boxShadow: [
                BoxShadow(
                  color: Colors.black.withOpacity(0.08),
                  blurRadius: 6,
                  offset: const Offset(0, 2),
                ),
              ],
            ),
            child: const Column(
              children: [
                Text(
                  'PHẦN THƯỞNG',
                  style: TextStyle(
                    fontSize: 11,
                    fontWeight: FontWeight.bold,
                    color: Colors.black87,
                  ),
                ),
                Text(
                  'Mở khóa',
                  style: TextStyle(
                    fontSize: 10,
                    color: Colors.grey,
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildPracticeCard(String emoji, String title, String subtitle, Color color) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.circular(16),
        border: Border.all(color: color.withOpacity(0.3), width: 2),
        boxShadow: [
          BoxShadow(
            color: color.withOpacity(0.1),
            blurRadius: 8,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            emoji,
            style: const TextStyle(fontSize: 32),
          ),
          const SizedBox(height: 8),
          Text(
            title,
            style: TextStyle(
              fontSize: 15,
              fontWeight: FontWeight.bold,
              color: color,
            ),
          ),
          const SizedBox(height: 4),
          Text(
            subtitle,
            style: TextStyle(
              fontSize: 12,
              color: Colors.grey[600],
            ),
            maxLines: 2,
          ),
        ],
      ),
    );
  }
}

enum LessonStatus { locked, available, inProgress, completed }